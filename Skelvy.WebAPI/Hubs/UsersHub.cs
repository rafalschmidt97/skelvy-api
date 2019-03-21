using System;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Meetings.Commands.AddMeetingChatMessage;
using Skelvy.Infrastructure.Notifications;

namespace Skelvy.WebAPI.Hubs
{
  public class UsersHub : BaseHub
  {
    public UsersHub(IMediator mediator)
      : base(mediator)
    {
    }

    public async Task SendMessage(AddMeetingChatMessageCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request, Context.ConnectionAborted);
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
