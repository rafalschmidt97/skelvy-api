using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Notifications.Infrastructure;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class SocketNotificationsService : ISocketNotificationsService
  {
    private readonly SignalRBackplane _socket;

    public SocketNotificationsService(SignalRBackplane socket)
    {
      _socket = socket;
    }

    public async Task BroadcastUserSentMessage(UserSentMessageAction action)
    {
      await SendNotification("UserSentMessage", action.UsersId, new MessageDto
      {
        Id = action.MeetingId,
        Text = action.Text,
        Date = action.Date,
        AttachmentUrl = action.AttachmentUrl,
        UserId = action.UserId,
        MeetingId = action.MeetingId,
      });
    }

    public async Task BroadcastUserJoinedMeeting(UserJoinedMeetingAction action)
    {
      await SendNotification("UserJoinedMeeting", action.UsersId, new { action.UserId });
    }

    public async Task BroadcastUserFoundMeeting(UserFoundMeetingAction action)
    {
      await SendNotification("UserFoundMeeting", action.UsersId);
    }

    public async Task BroadcastUserLeftMeeting(UserLeftMeetingAction action)
    {
      await SendNotification("UserLeftMeeting", action.UsersId, new { action.UserId });
    }

    public async Task BroadcastMeetingAborted(MeetingAbortedAction action)
    {
      await SendNotification("MeetingAborted", action.UsersId, new { action.UserId });
    }

    public async Task BroadcastMeetingRequestExpired(MeetingRequestExpiredAction action)
    {
      await SendNotification("MeetingRequestExpired", action.UsersId);
    }

    public async Task BroadcastMeetingExpired(MeetingExpiredAction action)
    {
      await SendNotification("MeetingExpired", action.UsersId);
    }

    public async Task BroadcastUserRemoved(UserRemovedAction action)
    {
      await SendNotification("UserRemoved", new[] { action.UserId });
    }

    public async Task BroadcastUserDisabled(UserDisabledAction action)
    {
      await SendNotification("UserDisabled", new[] { action.UserId });
    }

    private async Task SendNotification(string action, IEnumerable<int> usersId, object data = null)
    {
      await _socket.PublishMessage(new SocketMessage(usersId, action, data));
    }
  }
}
