using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Skelvy.Application.Core.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Domain.Entities;
using Skelvy.WebAPI.Hubs;

namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class NotificationsService : INotificationsService
  {
    private readonly IHubContext<MeetingHub> _hubContext;

    public NotificationsService(IHubContext<MeetingHub> hubContext)
    {
      _hubContext = hubContext;
    }

    public async Task SendMessage(MeetingChatMessage message, CancellationToken cancellationToken)
    {
      await _hubContext.Clients.Group(MeetingHub.GetGroupName(message)).SendAsync("ReceiveMessage", message, cancellationToken);
    }

    public async Task SendMessages(ICollection<MeetingChatMessageDto> messages, int userid, CancellationToken cancellationToken)
    {
      await _hubContext.Clients.User(userid.ToString()).SendAsync("ReceiveMessages", messages, cancellationToken);
    }
  }
}
