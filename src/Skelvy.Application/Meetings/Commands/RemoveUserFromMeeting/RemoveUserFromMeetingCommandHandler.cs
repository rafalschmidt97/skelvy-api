using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.UserRemovedFromMeeting;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.RemoveUserFromMeeting
{
  public class RemoveUserFromMeetingCommandHandler : CommandHandler<RemoveUserFromMeetingCommand>
  {
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMediator _mediator;

    public RemoveUserFromMeetingCommandHandler(
      IMeetingsRepository meetingsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMediator mediator)
    {
      _meetingsRepository = meetingsRepository;
      _groupUsersRepository = groupUsersRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(RemoveUserFromMeetingCommand request)
    {
      var (meeting, removedGroupUser) = await ValidateData(request);

      removedGroupUser.Remove();
      await _groupUsersRepository.Update(removedGroupUser);

      await _mediator.Publish(
        new UserRemovedFromMeetingEvent(request.UserId, removedGroupUser.UserId, meeting.GroupId, meeting.Id));

      return Unit.Value;
    }

    private async Task<(Meeting, GroupUser)> ValidateData(RemoveUserFromMeetingCommand request)
    {
      var meeting = await _meetingsRepository.FindOne(request.MeetingId);

      if (meeting == null)
      {
        throw new NotFoundException(nameof(Meeting), request.MeetingId);
      }

      var groupUser = await _groupUsersRepository.FindOneByUserIdAndGroupId(request.UserId, meeting.GroupId);

      if (groupUser == null)
      {
        throw new NotFoundException($"Entity {nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {meeting.GroupId}) not found.");
      }

      var removedGroupUser = await _groupUsersRepository.FindOneByUserIdAndGroupId(request.RemovedUserId, meeting.GroupId);

      if (removedGroupUser == null)
      {
        throw new NotFoundException($"Entity {nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {meeting.GroupId}) not found.");
      }

      if (!groupUser.CanRemoveUserFromGroup(removedGroupUser, meeting))
      {
        throw new ForbiddenException($"Entity {nameof(GroupUser)}(Id = {groupUser.Id}) does not have permission to remove user");
      }

      return (meeting, removedGroupUser);
    }
  }
}
