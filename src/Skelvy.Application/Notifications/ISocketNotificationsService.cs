using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Notifications
{
  public interface ISocketNotificationsService
  {
    Task BroadcastUserSentMeetingChatMessage(MeetingChatMessage message, IEnumerable<int> usersId);
    Task BroadcastUserJoinedMeeting(MeetingUser user, IEnumerable<int> usersId);
    Task BroadcastUserFoundMeeting(IEnumerable<int> usersId);
    Task BroadcastUserLeftMeeting(MeetingUser user, IEnumerable<int> usersId);
    Task BroadcastMeetingRequestExpired(IEnumerable<int> usersId);
    Task BroadcastMeetingExpired(IEnumerable<int> usersId);
  }
}
