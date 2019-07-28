using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.MeetingAborted;
using Skelvy.Application.Meetings.Events.UserLeftMeeting;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Events.UserDisabled;
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
    private readonly IMediator _mediator;

    public DisableUserCommandHandler(
      IUsersRepository usersRepository,
      IMeetingUsersRepository meetingUsersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMediator mediator)
    {
      _usersRepository = usersRepository;
      _meetingUsersRepository = meetingUsersRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _mediator = mediator;
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

      await _mediator.Publish(new UserDisabledEvent(user.Id, request.Reason, user.Email, user.Language));
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

          var meetingAborted = false;

          if (meetingUsers.Count == 2)
          {
            var anotherUserDetails = meetingUsers.First(x => x.UserId != meetingUser.UserId);

            anotherUserDetails.Abort();
            anotherUserDetails.MeetingRequest.MarkAsSearching();
            meetingUser.Meeting.Abort();

            await _meetingUsersRepository.Update(anotherUserDetails);
            await _meetingRequestsRepository.Update(anotherUserDetails.MeetingRequest);
            await _meetingsRepository.Update(meetingUser.Meeting);

            meetingAborted = true;
          }

          transaction.Commit();

          if (!meetingAborted)
          {
            await _mediator.Publish(new UserLeftMeetingEvent(meetingUser.UserId, meetingUser.MeetingId));
          }
          else
          {
            if (userDetails.ModifiedAt != null)
            {
              await _mediator.Publish(
                new MeetingAbortedEvent(meetingUser.UserId, meetingUser.MeetingId, userDetails.ModifiedAt.Value));
            }
            else
            {
              throw new InternalServerErrorException(
                $"Entity {nameof(MeetingUser)}(UserId = {meetingUser.UserId}) has modified date null after leaving");
            }
          }
        }
      }
    }
  }
}
