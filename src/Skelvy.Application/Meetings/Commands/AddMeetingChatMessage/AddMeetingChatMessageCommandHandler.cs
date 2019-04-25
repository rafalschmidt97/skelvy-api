using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommandHandler : CommandHandler<AddMeetingChatMessageCommand>
  {
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly INotificationsService _notifications;

    public AddMeetingChatMessageCommandHandler(IMeetingUsersRepository meetingUsersRepository, INotificationsService notifications)
    {
      _meetingUsersRepository = meetingUsersRepository;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(AddMeetingChatMessageCommand request)
    {
      var meetingUser = await _meetingUsersRepository.FindOneByUserId(request.UserId);

      if (meetingUser == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingUser)}(UserId = {request.UserId}) not found.");
      }

      var message = new MeetingChatMessage(request.Message, request.Date, meetingUser.UserId, meetingUser.MeetingId);
      _meetingUsersRepository.Context.MeetingChatMessages.Add(message);

      await _meetingUsersRepository.Context.SaveChangesAsync();
      await BroadcastMessage(message);
      return Unit.Value;
    }

    private async Task BroadcastMessage(MeetingChatMessage message)
    {
      var meetingUsers = await _meetingUsersRepository.FindAllByMeetingId(message.MeetingId);
      var meetingUserIds = meetingUsers.Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserSentMeetingChatMessage(message, meetingUserIds);
    }
  }
}
