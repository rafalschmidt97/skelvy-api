using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Skelvy.Common.Serializers;
using Skelvy.Infrastructure.Notifications;
using Skelvy.WebAPI.Hubs;
using StackExchange.Redis;

namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class SignalRBackplane
  {
    private readonly ISubscriber _subscriber;
    private readonly IHubContext<UsersHub> _hubContext;
    private readonly ILogger<SignalRBackplane> _logger;
    private bool _initialized;

    public SignalRBackplane(
      IConnectionMultiplexer redis,
      IHubContext<UsersHub> hubContext,
      ILogger<SignalRBackplane> logger)
    {
      _subscriber = redis.GetSubscriber();
      _hubContext = hubContext;
      _logger = logger;
    }

    public void Start()
    {
      InitializeConnection();
      SubscribeMessages();
    }

    public async Task PublishMessage(SocketMessage message)
    {
      await _subscriber.PublishAsync("Message", message.JsonSerialize());
    }

    public async Task ConnectUser(int userId)
    {
      await _subscriber.PublishAsync("ConnectUser", userId);
    }

    public async Task DisconnectUser(int userId)
    {
      await _subscriber.PublishAsync("DisconnectUser", userId);
    }

    private void InitializeConnection()
    {
      _subscriber.Subscribe($"Init-{Program.InstanceId}", (channel, action) =>
      {
        if (!_initialized)
        {
          _initialized = true;
          var connections = ((string)action).JsonDeserialize<HashSet<int>>();
          NotificationsService.Connections.UnionWith(connections);
        }
      });

      _subscriber.Subscribe("Init", (channel, action) =>
      {
        var instanceId = (string)action;

        if (instanceId != Program.InstanceId && _initialized)
        {
          _subscriber.Publish($"Init-{instanceId}", NotificationsService.Connections.JsonSerialize());
        }
      });

      _subscriber.Publish("Init", Program.InstanceId);

      Task.Run(async () =>
      {
        await Task.Delay(TimeSpan.FromSeconds(10));

        if (!_initialized)
        {
          _initialized = true;
          _logger.LogWarning("API has not received initialize ping. It assumes that this is the first instance.");
        }
      });
    }

    private void SubscribeMessages()
    {
      _subscriber.Subscribe("Message", (channel, action) =>
      {
        var message = ((string)action).JsonDeserialize<SocketMessage>();
        Task.Run(() => SendNotificationToOnline(message.UsersId, message.Action, message.Data));
      });

      _subscriber.Subscribe("ConnectUser", (channel, action) =>
      {
        var userId = (int)action;
        NotificationsService.Connections.Add(userId);
      });

      _subscriber.Subscribe("DisconnectUser", (channel, action) =>
      {
        var userId = (int)action;
        NotificationsService.Connections.Remove(userId);
      });
    }

    private async Task SendNotificationToOnline(IEnumerable<int> usersId, string action, object data)
    {
      var onlineUsersId = NotificationsService.Connections.Where(x => usersId.Any(y => y == x));
      await SendNotification(onlineUsersId, action, data);
    }

    private async Task SendNotification(IEnumerable<int> usersId, string action, object data)
    {
      await _hubContext.Clients
        .Users(usersId.Select(x => x.ToString()).ToList().AsReadOnly())
        .SendAsync(action, data);
    }
  }
}
