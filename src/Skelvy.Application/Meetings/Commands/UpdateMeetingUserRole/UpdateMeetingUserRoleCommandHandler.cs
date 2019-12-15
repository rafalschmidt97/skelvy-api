using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Events.MeetingUserRoleUpdated;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.UpdateMeetingUserRole
{
  public class UpdateMeetingUserRoleCommandHandler : CommandHandler<UpdateMeetingUserRoleCommand>
  {
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMediator _mediator;

    public UpdateMeetingUserRoleCommandHandler(
      IMeetingsRepository meetingsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMediator mediator)
    {
      _meetingsRepository = meetingsRepository;
      _groupUsersRepository = groupUsersRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(UpdateMeetingUserRoleCommand request)
    {
      var groupUser = await ValidateData(request);

      groupUser.UpdateRole(request.Role);
      await _groupUsersRepository.Update(groupUser);
      await _mediator.Publish(
        new MeetingUserRoleUpdatedEvent(
          request.UserId,
          request.MeetingId,
          groupUser.GroupId,
          request.UpdatedUserId,
          request.Role));

      return Unit.Value;
    }

    private async Task<GroupUser> ValidateData(UpdateMeetingUserRoleCommand request)
    {
      var meeting = await _meetingsRepository.FindOne(request.MeetingId);

      if (meeting == null)
      {
        throw new NotFoundException(nameof(Meeting), request.MeetingId);
      }

      var groupUser = await _groupUsersRepository.FindOneByUserIdAndGroupId(request.UserId, meeting.GroupId);

      if (groupUser == null)
      {
        throw new NotFoundException($"{nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {meeting.GroupId}) not found.");
      }

      var removedGroupUser = await _groupUsersRepository.FindOneByUserIdAndGroupId(request.UpdatedUserId, meeting.GroupId);

      if (removedGroupUser == null)
      {
        throw new NotFoundException($"{nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {meeting.GroupId}) not found.");
      }

      if (!groupUser.CanUpdateRole(removedGroupUser, request.Role))
      {
        throw new ForbiddenException($"{nameof(GroupUser)}({groupUser.Id}) does not have permission to update user role.");
      }

      return removedGroupUser;
    }
  }
}
