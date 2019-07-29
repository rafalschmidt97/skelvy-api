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

    private async Task LeaveMeetings(User user) // Same logic as LeaveMeetingCommand
    {
      var meetingUser = await _groupUsersRepository.FindOneWithGroupByUserId(user.Id);

      if (meetingUser != null)
      {
        var meetingUsers = await _groupUsersRepository.FindAllWithRequestByGroupId(meetingUser.GroupId);

        var userDetails = meetingUsers.First(x => x.UserId == meetingUser.UserId);

        using (var transaction = _usersRepository.BeginTransaction())
        {
          userDetails.Leave();
          userDetails.MeetingRequest.Abort();

          await _groupUsersRepository.Update(userDetails);
          await _meetingRequestsRepository.Update(userDetails.MeetingRequest);

          var meetingAborted = false;

          if (meetingUsers.Count == 2)
          {
            var anotherUserDetails = meetingUsers.First(x => x.UserId != meetingUser.UserId);

            anotherUserDetails.Abort();
            anotherUserDetails.MeetingRequest.MarkAsSearching();
            var meeting = await _meetingsRepository.FindOneWithGroupByGroupId(meetingUser.GroupId);
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
            await _mediator.Publish(new UserLeftMeetingEvent(meetingUser.UserId, meetingUser.GroupId));
          }
          else
          {
            if (userDetails.ModifiedAt != null)
            {
              await _mediator.Publish(
                new MeetingAbortedEvent(meetingUser.UserId, meetingUser.GroupId, userDetails.ModifiedAt.Value));
            }
            else
            {
              throw new InternalServerErrorException(
                $"Entity {nameof(GroupUser)}(UserId = {meetingUser.UserId}) has modified date null after leaving");
            }
          }
        }
      }
    }
  }
}
