using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Commands.DisableUser
{
  public class DisableUserCommandHandler : CommandHandler<DisableUserCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly INotificationsService _notifications;

    public DisableUserCommandHandler(
      IUsersRepository usersRepository,
      IMeetingUsersRepository meetingUsersRepository,
      INotificationsService notifications)
    {
      _usersRepository = usersRepository;
      _meetingUsersRepository = meetingUsersRepository;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(DisableUserCommand request)
    {
      var user = await _usersRepository.FindOne(request.Id);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.Id);
      }

      if (user.IsDisabled)
      {
        throw new ConflictException($"Entity {nameof(User)}(Id = {request.Id}) is already disabled.");
      }

      await LeaveMeetings(user);
      user.Disable(request.Reason);

      await _usersRepository.Context.SaveChangesAsync();
      await _notifications.BroadcastUserDisabled(user, request.Reason);

      return Unit.Value;
    }

    private async Task LeaveMeetings(User user) // Same logic as LeaveMeetingCommand
    {
      var meetingUser = await _meetingUsersRepository.FindOneWithMeetingByUserId(user.Id);

      if (meetingUser != null)
      {
        var meetingUsers = await _meetingUsersRepository.FindAllWithMeetingRequestByMeetingId(meetingUser.MeetingId);

        var userDetails = meetingUsers.First(x => x.UserId == meetingUser.UserId);

        userDetails.Leave();
        userDetails.MeetingRequest.Abort();

        if (meetingUsers.Count == 2)
        {
          var anotherUserDetails = meetingUsers.First(x => x.UserId != meetingUser.UserId);

          anotherUserDetails.Leave();
          anotherUserDetails.MeetingRequest.MarkAsSearching();
          meetingUser.Meeting.Abort();

          await _meetingUsersRepository.Context.SaveChangesAsync();
          await BroadcastUserLeftMeeting(meetingUser, meetingUsers);
        }

        await _meetingUsersRepository.Context.SaveChangesAsync();
        await BroadcastUserLeftMeeting(meetingUser, meetingUsers);
      }
    }

    private async Task BroadcastUserLeftMeeting(MeetingUser meetingUser, IEnumerable<MeetingUser> meetingUsers)
    {
      var meetingUserIds = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserLeftMeeting(meetingUser, meetingUserIds);
    }
  }
}
