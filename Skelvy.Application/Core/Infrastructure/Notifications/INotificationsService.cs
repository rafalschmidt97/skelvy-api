using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Core.Infrastructure.Notifications
{
  public interface INotificationsService
  {
    Task BroadcastMessage(MeetingChatMessageDto message, CancellationToken cancellationToken);
    Task BroadcastMessages(ICollection<MeetingChatMessageDto> messages, int userid, CancellationToken cancellationToken);
    Task BroadcastUserAddedToMeeting(int meetingId, CancellationToken cancellationToken);
    Task BroadcastMeetingFound(int userId, CancellationToken cancellationToken);
    Task BroadcastUserLeftMeeting(int meetingId, CancellationToken cancellationToken);
  }
}
