using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.MeetingAborted;
using Skelvy.Application.Meetings.Events.UserLeftMeeting;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Events.UserRemoved;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Commands.RemoveUser
{
  public class RemoveUserCommandHandler : CommandHandler<RemoveUserCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IGroupsRepository _groupsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMediator _mediator;

    public RemoveUserCommandHandler(
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

    public override async Task<Unit> Handle(RemoveUserCommand request)
    {
      var user = await _usersRepository.FindOne(request.Id);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.Id);
      }

      await LeaveMeetings(user);
      user.Remove(DateTimeOffset.UtcNow.AddMonths(3));

      await _usersRepository.Update(user);

      await _mediator.Publish(new UserRemovedEvent(user.Id, user.Email, user.Language));
      return Unit.Value;
    }

    private async Task LeaveMeetings(User user) // Same logic in DisableUserCommandHandler
    {
      var groupUsers = await _groupUsersRepository.FindAllByUserId(user.Id);

      if (groupUsers.Any())
      {
        foreach (var groupUser in groupUsers)
        {
          var groupUsersDetails = await _groupUsersRepository.FindAllWithRequestByGroupId(groupUser.GroupId);
          var groupUserDetails = groupUsersDetails.First(x => x.UserId == groupUser.UserId);

          using (var transaction = _usersRepository.BeginTransaction())
          {
            groupUserDetails.Leave();

            if (groupUserDetails.MeetingRequest != null)
            {
              groupUserDetails.MeetingRequest.Abort();
              await _meetingRequestsRepository.Update(groupUserDetails.MeetingRequest);
            }

            await _groupUsersRepository.Update(groupUserDetails);

            var meetingAborted = false;

            if (groupUsersDetails.Count == 2)
            {
              var anotherUserDetails = groupUsersDetails.First(x => x.UserId != groupUser.UserId);

              anotherUserDetails.Abort();

              if (anotherUserDetails.MeetingRequest != null)
              {
                anotherUserDetails.MeetingRequest.MarkAsSearching();
                await _meetingRequestsRepository.Update(anotherUserDetails.MeetingRequest);
              }

              await _groupUsersRepository.Update(anotherUserDetails);

              var group = await _groupsRepository.FindOne(groupUser.GroupId);

              if (group != null)
              {
                group.Abort();
                await _groupsRepository.Update(group);
              }

              var meeting = await _meetingsRepository.FindOneByGroupId(groupUser.GroupId);

              if (meeting != null)
              {
                meeting.Abort();
                await _meetingsRepository.Update(meeting);
              }

              meetingAborted = true;
            }

            transaction.Commit();

            if (!meetingAborted)
            {
              await _mediator.Publish(new UserLeftMeetingEvent(groupUser.UserId, groupUser.GroupId));
            }
            else
            {
              if (groupUserDetails.ModifiedAt != null)
              {
                await _mediator.Publish(
                  new MeetingAbortedEvent(groupUser.UserId, groupUser.GroupId, groupUserDetails.ModifiedAt.Value));
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
}
