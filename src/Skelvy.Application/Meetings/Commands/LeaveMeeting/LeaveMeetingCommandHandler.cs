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
      var groupUser = await ValidateData(request);

      var groupUsers = await _groupUsersRepository.FindAllWithRequestByGroupId(groupUser.GroupId);
      var userDetails = groupUsers.First(x => x.UserId == groupUser.UserId);

      if (userDetails.MeetingRequest.IsSearching)
      {
        throw new InternalServerErrorException(
          $"Entity {nameof(MeetingRequest)}(UserId = {request.UserId}) is marked as '{MeetingRequestStatusType.Searching}' " +
          $"while {nameof(GroupUser)} exists");
      }

      using (var transaction = _meetingsRepository.BeginTransaction())
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
          var meetingDetails = await _meetingsRepository.FindOneWithGroupByGroupId(groupUser.GroupId);
          meetingDetails.Abort();
          meetingDetails.Group.Abort();

          await _groupUsersRepository.Update(anotherUserDetails);
          await _meetingRequestsRepository.Update(anotherUserDetails.MeetingRequest);
          await _meetingsRepository.Update(meetingDetails);
          await _groupsRepository.Update(meetingDetails.Group);

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

      return Unit.Value;
    }

    private async Task<GroupUser> ValidateData(LeaveMeetingCommand request)
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

      return groupUser;
    }
  }
}
