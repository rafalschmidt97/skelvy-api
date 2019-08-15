using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.MeetingAborted;
using Skelvy.Application.Meetings.Events.UserLeftMeeting;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.LeaveGroup
{
  public class LeaveGroupCommandHandler : CommandHandler<LeaveGroupCommand>
  {
    private readonly IGroupsRepository _groupsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMediator _mediator;

    public LeaveGroupCommandHandler(
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

    public override async Task<Unit> Handle(LeaveGroupCommand request)
    {
      await ValidateData(request);

      var groupUsers = await _groupUsersRepository.FindAllByGroupId(request.GroupId);
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

        var groupAborted = false;

        if (groupUsers.Count == 2)
        {
          var meeting = await _meetingsRepository.FindOneByGroupId(groupUserDetails.GroupId);

          if (meeting != null)
          {
            meeting.Abort();
            await _meetingsRepository.Update(meeting);

            if (!meeting.IsPrivate)
            {
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

              groupAborted = true;
            }
          }
        }

        transaction.Commit();

        if (!groupAborted)
        {
          await _mediator.Publish(new UserLeftMeetingEvent(groupUserDetails.UserId, groupUserDetails.GroupId));
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

    private async Task ValidateData(LeaveGroupCommand request)
    {
      var groupUser = await _groupUsersRepository.FindOneWithGroupByUserIdAndGroupId(request.UserId, request.GroupId);

      if (groupUser == null)
      {
        throw new NotFoundException(nameof(GroupUser), request.UserId);
      }
    }
  }
}
