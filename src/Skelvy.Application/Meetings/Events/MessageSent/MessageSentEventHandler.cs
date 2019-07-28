using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Infrastructure.Repositories;

namespace Skelvy.Application.Meetings.Events.MessageSent
{
  public class MessageSentEventHandler : EventHandler<MessageSentEvent>
  {
    private readonly INotificationsService _notifications;
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingUsersRepository _meetingUsersRepository;

    public MessageSentEventHandler(
      INotificationsService notifications,
      IUsersRepository usersRepository,
      IMeetingUsersRepository meetingUsersRepository)
    {
      _notifications = notifications;
      _usersRepository = usersRepository;
      _meetingUsersRepository = meetingUsersRepository;
    }

    public override async Task<Unit> Handle(MessageSentEvent request)
    {
      var meetingUsers = await _meetingUsersRepository.FindAllByMeetingId(request.MeetingId);
      var broadcastUsersId = meetingUsers.Where(x => x.UserId != request.UserId).Select(x => x.UserId).ToList();

      var sender = await _usersRepository.FindOneWithDetails(request.UserId);

      await _notifications.BroadcastUserSentMessage(
        new UserSentMessageAction(
          request.MessageId,
          request.Message,
          request.Date,
          request.AttachmentUrl,
          request.UserId,
          sender.Profile.Name,
          request.MeetingId,
          broadcastUsersId));

      return Unit.Value;
    }
  }
}
