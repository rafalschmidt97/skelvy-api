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
    private readonly IGroupUsersRepository _groupUsersRepository;

    public MessageSentEventHandler(
      INotificationsService notifications,
      IUsersRepository usersRepository,
      IGroupUsersRepository groupUsersRepository)
    {
      _notifications = notifications;
      _usersRepository = usersRepository;
      _groupUsersRepository = groupUsersRepository;
    }

    public override async Task<Unit> Handle(MessageSentEvent request)
    {
      var meetingUsers = await _groupUsersRepository.FindAllByGroupId(request.Message.GroupId);
      var broadcastUsersId = meetingUsers.Where(x => x.UserId != request.Message.UserId).Select(x => x.UserId).ToList();

      var sender = await _usersRepository.FindOneWithDetails(request.Message.UserId);

      await _notifications.BroadcastUserSentMessage(
        new UserSentMessageAction(
          request.Type,
          new UserSentMessageActionDto(
            request.Message.MessageId,
            request.Message.Type,
            request.Message.Date,
            request.Message.Text,
            request.Message.AttachmentUrl,
            request.Message.Action,
            request.Message.UserId,
            sender.Profile.Name,
            request.Message.GroupId),
          request.Messages,
          broadcastUsersId));

      return Unit.Value;
    }
  }
}
