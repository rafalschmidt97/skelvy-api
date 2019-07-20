using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Infrastructure.Notifications;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Commands.DisableUser
{
  public class DisableUserCommandHandler : CommandHandler<DisableUserCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly INotificationsService _notifications;

    public DisableUserCommandHandler(
      IUsersRepository usersRepository,
      IMeetingUsersRepository meetingUsersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      INotificationsService notifications)
    {
      _usersRepository = usersRepository;
      _meetingUsersRepository = meetingUsersRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
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

      await _usersRepository.Update(user);
      await _notifications.BroadcastUserDisabled(new UserDisabledAction(user.Id, request.Reason, user.Email, user.Language));

      return Unit.Value;
    }

    private async Task LeaveMeetings(User user) // Same logic as LeaveMeetingCommand
    {
      var meetingUser = await _meetingUsersRepository.FindOneWithMeetingByUserId(user.Id);

      if (meetingUser != null)
      {
        var meetingUsers = await _meetingUsersRepository.FindAllWithMeetingRequestByMeetingId(meetingUser.MeetingId);

        var userDetails = meetingUsers.First(x => x.UserId == meetingUser.UserId);

        using (var transaction = _usersRepository.BeginTransaction())
        {
          userDetails.Leave();
          userDetails.MeetingRequest.Abort();

          await _meetingUsersRepository.Update(userDetails);
          await _meetingRequestsRepository.Update(userDetails.MeetingRequest);

          if (meetingUsers.Count == 2)
          {
            var anotherUserDetails = meetingUsers.First(x => x.UserId != meetingUser.UserId);

            anotherUserDetails.Abort();
            anotherUserDetails.MeetingRequest.MarkAsSearching();
            meetingUser.Meeting.Abort();

            await _meetingUsersRepository.Update(anotherUserDetails);
            await _meetingRequestsRepository.Update(anotherUserDetails.MeetingRequest);
            await _meetingsRepository.Update(meetingUser.Meeting);
          }

          transaction.Commit();
          await BroadcastUserLeftMeeting(meetingUser, meetingUsers);
        }
      }
    }

    private async Task BroadcastUserLeftMeeting(MeetingUser leftUser, IEnumerable<MeetingUser> meetingUsers)
    {
      var broadcastUsersId = meetingUsers.Where(x => x.UserId != leftUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserLeftMeeting(new UserLeftMeetingAction(leftUser.UserId, broadcastUsersId));
    }
  }
}
