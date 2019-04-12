using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Infrastructure.Notifications
{
  public interface IPushNotificationsService
  {
    Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, IEnumerable<int> userIds);
    Task BroadcastUserJoinedMeeting(MeetingUser user, IEnumerable<int> userIds);
    Task BroadcastUserFoundMeeting(IEnumerable<int> userIds);
    Task BroadcastUserLeftMeeting(MeetingUser user, IEnumerable<int> userIds);
    Task BroadcastMeetingRequestExpired(IEnumerable<int> userIds);
    Task BroadcastMeetingExpired(IEnumerable<int> userIds);
  }
}
