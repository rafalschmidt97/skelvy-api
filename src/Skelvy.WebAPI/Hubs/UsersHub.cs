using System;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Notifications;
using Skelvy.Common.Serializers;
using Skelvy.Infrastructure.Notifications;
using StackExchange.Redis;

namespace Skelvy.WebAPI.Hubs
{
  public class UsersHub : BaseHub
  {
    private readonly ISubscriber _subscriber;
    private readonly INotificationsService _notifications;

    public UsersHub(IMediator mediator, IConnectionMultiplexer redis, INotificationsService notifications)
      : base(mediator)
    {
      _notifications = notifications;
      _subscriber = redis.GetSubscriber();
      ListenForEvents();
    }

    public override Task OnConnectedAsync()
    {
      NotificationsService.Connections.Add(UserId);
      return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
      var userId = UserId;

      if (NotificationsService.IsConnected(userId))
      {
        NotificationsService.Connections.Remove(userId);
      }

      return base.OnDisconnectedAsync(exception);
    }

    private void ListenForEvents()
    {
      _subscriber.Subscribe("UserSentMessage", (channel, action) =>
      {
        var data = ((byte[])action).Deserialize<UserSentMessageAction>();
        _notifications.BroadcastUserSentMessage(data);
      });
    }
  }
}
