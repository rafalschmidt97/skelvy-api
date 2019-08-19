using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.MeetingAborted;
using Skelvy.Application.Meetings.Events.UserLeftMeeting;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;

namespace Skelvy.Application.Meetings.Commands.LeaveMeeting
{
  public class LeaveMeetingCommandHandler : CommandHandler<LeaveMeetingCommand>
  {
    private readonly IGroupsRepository _groupsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMediator _mediator;

    public LeaveMeetingCommandHandler(
      IGroupsRepository groupsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMediator mediator)
    {
      _groupsRepository = groupsRepository;
      _groupUsersRepository = groupUsersRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(LeaveMeetingCommand request)
    {
      var meeting = await ValidateData(request);

      var groupUsers = await _groupUsersRepository.FindAllByGroupId(meeting.GroupId);
      var groupUserDetails = groupUsers.First(x => x.UserId == request.UserId);

      using (var transaction = _groupUsersRepository.BeginTransaction())
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

        var meetingAborted = false;

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

          meetingAborted = true;
        }
        else if (groupUsers.Count == 1)
        {
          meeting.Abort();
          await _meetingsRepository.Update(meeting);

          var group = await _groupsRepository.FindOne(groupUserDetails.GroupId);

          if (group != null)
          {
            group.Abort();
            await _groupsRepository.Update(group);
          }

          meetingAborted = true;
        }
        else
        {
          if (groupUserDetails.Role == GroupUserRoleType.Owner || groupUserDetails.Role == GroupUserRoleType.Admin)
          {
            var otherUsers = groupUsers.Where(x => x.UserId != request.UserId).ToList();

            if (!otherUsers.Any(x => x.Role == GroupUserRoleType.Owner || x.Role == GroupUserRoleType.Admin))
            {
              foreach (var otherUser in otherUsers)
              {
                otherUser.UpdateRole(GroupUserRoleType.Admin);
              }

              await _groupUsersRepository.UpdateRange(otherUsers);
            }
          }
        }

        transaction.Commit();

        if (!meetingAborted)
        {
          await _mediator.Publish(new UserLeftMeetingEvent(groupUserDetails.UserId, groupUserDetails.GroupId, meeting.Id));
        }
        else
        {
          if (groupUserDetails.ModifiedAt != null)
          {
            await _mediator.Publish(
              new MeetingAbortedEvent(groupUserDetails.UserId, groupUserDetails.GroupId, groupUserDetails.ModifiedAt.Value));
          }
          else
          {
            throw new InternalServerErrorException(
              $"Entity {nameof(GroupUser)}(UserId = {groupUserDetails.UserId}) has modified date null after leaving");
          }
        }
      }

      return Unit.Value;
    }

    private async Task<Meeting> ValidateData(LeaveMeetingCommand request)
    {
      var meeting = await _meetingsRepository.FindOne(request.MeetingId);

      if (meeting == null)
      {
        throw new NotFoundException(nameof(Meeting), request.UserId);
      }

      var groupUser = await _groupUsersRepository.FindOneWithGroupByUserIdAndGroupId(request.UserId, meeting.GroupId);

      if (groupUser == null)
      {
        throw new NotFoundException(nameof(GroupUser), request.UserId);
      }

      return meeting;
    }
  }
}
