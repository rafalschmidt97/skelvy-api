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
    private readonly IMeetingRequestsRepository _requestsRepository;
    private readonly IMediator _mediator;

    public LeaveGroupCommandHandler(
      IGroupsRepository groupsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMeetingRequestsRepository requestsRepository,
      IMediator mediator)
    {
      _groupsRepository = groupsRepository;
      _groupUsersRepository = groupUsersRepository;
      _requestsRepository = requestsRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(LeaveGroupCommand request)
    {
      var groupUser = await ValidateData(request);

      var groupUsers = await _groupUsersRepository.FindAllWithGroupByGroupId(groupUser.GroupId);
      var groupUserDetails = groupUsers.First(x => x.UserId == groupUser.UserId);

      using (var transaction = _groupUsersRepository.BeginTransaction())
      {
        groupUserDetails.Leave();
        await _groupUsersRepository.Update(groupUserDetails);

        var groupAborted = false;

        if (groupUsers.Count == 2)
        {
          var anotherUserDetails = groupUsers.First(x => x.UserId != groupUser.UserId);
          anotherUserDetails.Abort();
          groupUserDetails.Group.Abort();

          await _groupUsersRepository.Update(anotherUserDetails);
          await _groupsRepository.Update(groupUserDetails.Group);

          groupAborted = true;
        }

        transaction.Commit();

        if (!groupAborted)
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

      return Unit.Value;
    }

    private async Task<GroupUser> ValidateData(LeaveGroupCommand request)
    {
      var groupUser = await _groupUsersRepository.FindOneWithGroupByUserIdAndGroupId(request.UserId, request.GroupId);

      if (groupUser == null)
      {
        throw new NotFoundException(nameof(GroupUser), request.UserId);
      }

      if (groupUser.MeetingRequestId != null)
      {
        var requestExists = await _requestsRepository.ExistsOneFoundByRequestId(groupUser.MeetingRequestId.Value);

        if (requestExists)
        {
          throw new ConflictException($"Entity {nameof(GroupUser)}(UserId = {request.UserId}) contains found request. Leave meeting instead.");
        }
      }

      return groupUser;
    }
  }
}
