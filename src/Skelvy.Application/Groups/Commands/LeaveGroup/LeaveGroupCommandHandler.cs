using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Events.GroupAborted;
using Skelvy.Application.Groups.Events.UserLeftGroup;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Groups.Commands.LeaveGroup
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

      await using var transaction = _groupUsersRepository.BeginTransaction();
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
        var group = await _groupsRepository.FindOne(groupUserDetails.GroupId);

        if (group != null)
        {
          group.Abort();
          await _groupsRepository.Update(group);
        }

        groupAborted = true;
      }

      transaction.Commit();

      if (!groupAborted)
      {
        await _mediator.Publish(new UserLeftGroupEvent(groupUserDetails.UserId, groupUserDetails.GroupId));
      }
      else if (groupUserDetails.ModifiedAt != null)
      {
        await _mediator.Publish(
          new GroupAbortedEvent(groupUserDetails.UserId, groupUserDetails.GroupId, groupUserDetails.ModifiedAt.Value));
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

      var existsMeeting = await _meetingsRepository.ExistsOneByGroupId(request.GroupId);

      if (existsMeeting)
      {
        throw new ConflictException($"{nameof(GroupUser)}(UserId = {request.UserId} is associated with meeting.");
      }
    }
  }
}
