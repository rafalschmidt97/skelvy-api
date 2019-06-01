using System;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Infrastructure.Notifications;

namespace Skelvy.WebAPI.Hubs
{
  public class UsersHub : BaseHub
  {
    public UsersHub(IMediator mediator)
      : base(mediator)
    {
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
  }
}
