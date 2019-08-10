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
    private readonly IGroupsRepository _groupsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMediator _mediator;

    public DisableUserCommandHandler(
      IUsersRepository usersRepository,
      IGroupsRepository groupsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMediator mediator)
    {
      _usersRepository = usersRepository;
      _groupsRepository = groupsRepository;
      _groupUsersRepository = groupUsersRepository;
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
      var groupUser = await _groupUsersRepository.FindOneWithGroupByUserId(user.Id);

      if (groupUser != null)
      {
        var groupUsers = await _groupUsersRepository.FindAllWithRequestByGroupId(groupUser.GroupId);

        var userDetails = groupUsers.First(x => x.UserId == groupUser.UserId);

        using (var transaction = _usersRepository.BeginTransaction())
        {
          userDetails.Leave();
          userDetails.MeetingRequest.Abort();

          await _groupUsersRepository.Update(userDetails);
          await _meetingRequestsRepository.Update(userDetails.MeetingRequest);

          var meetingAborted = false;

          if (groupUsers.Count == 2)
          {
            var anotherUserDetails = groupUsers.First(x => x.UserId != groupUser.UserId);

            anotherUserDetails.Abort();
            anotherUserDetails.MeetingRequest.MarkAsSearching();
            var meeting = await _meetingsRepository.FindOneWithGroupByGroupId(groupUser.GroupId);
            meeting.Abort();
            meeting.Group.Abort();

            await _groupUsersRepository.Update(anotherUserDetails);
            await _meetingRequestsRepository.Update(anotherUserDetails.MeetingRequest);
            await _meetingsRepository.Update(meeting);
            await _groupsRepository.Update(meeting.Group);

            meetingAborted = true;
          }

          transaction.Commit();

          if (!meetingAborted)
          {
            await _mediator.Publish(new UserLeftMeetingEvent(groupUser.UserId, groupUser.GroupId));
          }
          else
          {
            if (userDetails.ModifiedAt != null)
            {
              await _mediator.Publish(
                new MeetingAbortedEvent(groupUser.UserId, groupUser.GroupId, userDetails.ModifiedAt.Value));
            }
            else
            {
              throw new InternalServerErrorException(
                $"Entity {nameof(GroupUser)}(UserId = {groupUser.UserId}) has modified date null after leaving");
            }
          }
        }
      }
    }
  }
}
