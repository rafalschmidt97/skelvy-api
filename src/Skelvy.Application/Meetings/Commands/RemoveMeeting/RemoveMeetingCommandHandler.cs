using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.MeetingAborted;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;

namespace Skelvy.Application.Meetings.Commands.RemoveMeeting
{
  public class RemoveMeetingCommandHandler : CommandHandler<RemoveMeetingCommand>
  {
    private readonly IGroupsRepository _groupsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMeetingInvitationsRepository _meetingInvitationsRepository;
    private readonly IMediator _mediator;

    public RemoveMeetingCommandHandler(
      IGroupsRepository groupsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingInvitationsRepository meetingInvitationsRepository,
      IMediator mediator)
    {
      _groupsRepository = groupsRepository;
      _groupUsersRepository = groupUsersRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingInvitationsRepository = meetingInvitationsRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(RemoveMeetingCommand request)
    {
      var meeting = await ValidateData(request);

      using (var transaction = _meetingsRepository.BeginTransaction())
      {
        meeting.Abort();
        await _meetingsRepository.Update(meeting);
        meeting.Group.Abort();
        await _groupsRepository.Update(meeting.Group);
        foreach (var groupUser in meeting.Group.Users)
        {
          groupUser.Abort();

          if (groupUser.MeetingRequestId != null)
          {
            var meetingRequest = await _meetingRequestsRepository.FindOne(groupUser.MeetingRequestId.Value);

            if (meetingRequest != null)
            {
              meetingRequest.Abort();
              await _meetingRequestsRepository.Update(meetingRequest);
            }
          }
        }

        await _groupUsersRepository.UpdateRange(meeting.Group.Users);

        var invitations = await _meetingInvitationsRepository.FindAllByMeetingId(meeting.Id);

        foreach (var invitation in invitations)
        {
          invitation.Abort();
        }

        await _meetingInvitationsRepository.UpdateRange(invitations);

        transaction.Commit();

        if (meeting.ModifiedAt != null)
        {
          await _mediator.Publish(
            new MeetingAbortedEvent(request.UserId, meeting.GroupId, meeting.ModifiedAt.Value));
        }
      }

      return Unit.Value;
    }

    private async Task<Meeting> ValidateData(RemoveMeetingCommand request)
    {
      var meeting = await _meetingsRepository.FindOneWithGroupUsersByMeetingId(request.MeetingId);

      if (meeting == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingRequest)}(UserId = {request.UserId}) not found.");
      }

      var userExists = await _groupUsersRepository
        .ExistsOneByUserIdAndGroupIdAndRole(request.UserId, meeting.GroupId, GroupUserRoleType.Owner);

      if (!userExists)
      {
        throw new NotFoundException(
          $"Entity {nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {meeting.GroupId}, Role = {GroupUserRoleType.Owner}) not found.");
      }

      return meeting;
    }
  }
}
