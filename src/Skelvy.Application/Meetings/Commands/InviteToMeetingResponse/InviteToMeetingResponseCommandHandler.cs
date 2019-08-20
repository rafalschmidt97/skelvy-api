using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Events.UserRespondedMeetingInvitation;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.InviteToMeetingResponse
{
  public class InviteToMeetingResponseCommandHandler : CommandHandler<InviteToMeetingResponseCommand>
  {
    private readonly IMeetingInvitationsRepository _meetingInvitationsRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMediator _mediator;

    public InviteToMeetingResponseCommandHandler(
      IMeetingInvitationsRepository meetingInvitationsRepository,
      IMeetingsRepository meetingsRepository,
      IGroupUsersRepository groupUsersRepository,
      IUsersRepository usersRepository,
      IMediator mediator)
    {
      _meetingInvitationsRepository = meetingInvitationsRepository;
      _meetingsRepository = meetingsRepository;
      _groupUsersRepository = groupUsersRepository;
      _usersRepository = usersRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(InviteToMeetingResponseCommand request)
    {
      var (meeting, meetingInvitation) = await ValidateData(request);

      using (var transaction = _meetingInvitationsRepository.BeginTransaction())
      {
        if (request.IsAccepted)
        {
          meetingInvitation.Accept();
        }
        else
        {
          meetingInvitation.Deny();
        }

        var groupUser = new GroupUser(meeting.GroupId, meetingInvitation.InvitedUserId);

        await _meetingInvitationsRepository.Update(meetingInvitation);
        await _groupUsersRepository.Add(groupUser);

        transaction.Commit();

        await _mediator.Publish(
          new UserRespondedMeetingInvitationEvent(meetingInvitation.Id, request.IsAccepted, meetingInvitation.InvitingUserId, meetingInvitation.InvitedUserId));
      }

      return Unit.Value;
    }

    private async Task<(Meeting, MeetingInvitation)> ValidateData(InviteToMeetingResponseCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException($"{nameof(Profile)}(UserId = {request.UserId}) not found.");
      }

      var meetingInvitation = await _meetingInvitationsRepository.FindOneByRequestId(request.InvitationId);

      if (meetingInvitation == null)
      {
        throw new NotFoundException(nameof(MeetingInvitation), request.InvitationId);
      }

      if (meetingInvitation.InvitedUserId != request.UserId)
      {
        throw new ConflictException(
          $"{nameof(MeetingInvitation)}(RequestId = {request.InvitationId}) not belong to {nameof(User)}(Id = {request.UserId}).");
      }

      var meeting = await _meetingsRepository.FindOne(meetingInvitation.MeetingId);

      if (meeting == null)
      {
        throw new NotFoundException(nameof(Meeting), meetingInvitation.MeetingId);
      }

      return (meeting, meetingInvitation);
    }
  }
}
