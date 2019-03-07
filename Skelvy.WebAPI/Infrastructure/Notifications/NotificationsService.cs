using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Skelvy.Application.Core.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Queries;
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

    public async Task BroadcastMessage(MeetingChatMessageDto message, CancellationToken cancellationToken)
    {
      await _hubContext.Clients.Group(MeetingHub.GetGroupName(message.MeetingId)).SendAsync("ReceiveMessage", message, cancellationToken);
    }

    public async Task BroadcastMessages(ICollection<MeetingChatMessageDto> messages, int userid, CancellationToken cancellationToken)
    {
      await _hubContext.Clients.User(userid.ToString()).SendAsync("ReceiveMessages", messages, cancellationToken);
    }

    public async Task BroadcastUserAddedToMeeting(int meetingId, CancellationToken cancellationToken)
    {
      await _hubContext.Clients.Group(MeetingHub.GetGroupName(meetingId)).SendAsync("UserAddedToMeeting", cancellationToken);
    }

    public async Task BroadcastMeetingFound(int userId, CancellationToken cancellationToken)
    {
      await _hubContext.Clients.User(userId.ToString()).SendAsync("MeetingFound", cancellationToken);
    }
  }
}
