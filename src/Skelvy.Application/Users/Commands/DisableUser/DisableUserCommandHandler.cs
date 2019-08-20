using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Events.UserLeftGroup;
using Skelvy.Application.Groups.Infrastructure.Repositories;
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

    private async Task LeaveMeetings(User user) // Same logic in RemoveUserCommandHandler
    {
      var groupUsers = await _groupUsersRepository.FindAllByUserId(user.Id);

      if (groupUsers.Any())
      {
        foreach (var groupUser in groupUsers)
        {
          var groupUsersDetails = await _groupUsersRepository.FindAllWithGroupAndRequestByGroupId(groupUser.GroupId);
          var groupUserDetails = groupUsersDetails.First(x => x.UserId == groupUser.UserId);

          using (var transaction = _usersRepository.BeginTransaction())
          {
            groupUserDetails.Leave();
            await _groupUsersRepository.Update(groupUserDetails);

            if (groupUserDetails.MeetingRequestId != null)
            {
              var meetingRequest = await _meetingRequestsRepository.FindOne(groupUserDetails.MeetingRequestId.Value);

              if (meetingRequest != null)
              {
                meetingRequest.Abort();
                await _meetingRequestsRepository.Update(meetingRequest);
              }
            }

            var meeting = await _meetingsRepository.FindOneByGroupId(groupUserDetails.GroupId);
            var abortedMeeting = false;

            if (meeting != null)
            {
              if (groupUsers.Count == 2 && !meeting.IsPrivate)
              {
                meeting.Abort();
                await _meetingsRepository.Update(meeting);

                var anotherUserDetails = groupUsers.First(x => x.UserId != groupUserDetails.UserId);
                anotherUserDetails.Abort();
                await _groupUsersRepository.Update(anotherUserDetails);

                if (anotherUserDetails.MeetingRequestId != null)
                {
                  var anotherMeetingRequest = await _meetingRequestsRepository.FindOne(anotherUserDetails.MeetingRequestId.Value);

                  if (anotherMeetingRequest != null)
                  {
                    anotherMeetingRequest.MarkAsSearching();
                    await _meetingRequestsRepository.Update(anotherMeetingRequest);
                  }
                }

                var group = await _groupsRepository.FindOne(groupUserDetails.GroupId);

                if (group != null)
                {
                  group.Abort();
                  await _groupsRepository.Update(group);
                }

                abortedMeeting = true;
              }
            }

            transaction.Commit();

            if (meeting != null)
            {
              if (!abortedMeeting)
              {
                await _mediator.Publish(new UserLeftMeetingEvent(groupUserDetails.UserId, groupUserDetails.GroupId, meeting.Id));
              }
              else if (groupUserDetails.ModifiedAt != null)
              {
                await _mediator.Publish(
                  new MeetingAbortedEvent(groupUserDetails.UserId, groupUserDetails.GroupId, groupUserDetails.ModifiedAt.Value));
              }
            }
            else
            {
              await _mediator.Publish(new UserLeftGroupEvent(groupUserDetails.UserId, groupUserDetails.GroupId));
            }
          }
        }
      }
    }
  }
}
