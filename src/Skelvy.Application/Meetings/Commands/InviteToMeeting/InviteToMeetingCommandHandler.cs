using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Events.UserSentMeetingInvitation;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Commands.InviteToMeeting
{
  public class InviteToMeetingCommandHandler : CommandHandler<InviteToMeetingCommand>
  {
    private readonly IMeetingInvitationsRepository _meetingInvitationsRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IRelationsRepository _relationsRepository;
    private readonly IMediator _mediator;

    public InviteToMeetingCommandHandler(
      IMeetingInvitationsRepository meetingInvitationsRepository,
      IMeetingsRepository meetingsRepository,
      IGroupUsersRepository groupUsersRepository,
      IUsersRepository usersRepository,
      IRelationsRepository relationsRepository,
      IMediator mediator)
    {
      _meetingInvitationsRepository = meetingInvitationsRepository;
      _meetingsRepository = meetingsRepository;
      _groupUsersRepository = groupUsersRepository;
      _usersRepository = usersRepository;
      _relationsRepository = relationsRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(InviteToMeetingCommand request)
    {
      await ValidateData(request);

      var meetingInvitation = new MeetingInvitation(request.UserId, request.InvitingUserId, request.MeetingId);

      await _meetingInvitationsRepository.Add(meetingInvitation);

      await _mediator.Publish(new UserSentMeetingInvitationEvent(
        meetingInvitation.Id, meetingInvitation.InvitingUserId, meetingInvitation.InvitedUserId, meetingInvitation.MeetingId));

      return Unit.Value;
    }

    private async Task ValidateData(InviteToMeetingCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var relatedUserExists = await _usersRepository.ExistsOne(request.InvitingUserId);

      if (!relatedUserExists)
      {
        throw new NotFoundException(nameof(User), request.InvitingUserId);
      }

      var meeting = await _meetingsRepository.FindOne(request.MeetingId);

      if (meeting == null)
      {
        throw new NotFoundException(nameof(Meeting), request.MeetingId);
      }

      var groupUserExists = await _groupUsersRepository.ExistsOneByUserIdAndGroupId(request.InvitingUserId, meeting.GroupId);

      if (groupUserExists)
      {
        throw new ConflictException($"{nameof(GroupUser)}(UserId = {request.InvitingUserId}) already exists.");
      }

      var existsMeetingInvitation = await _meetingInvitationsRepository
        .ExistsOneByInvitedUserIdAndMeetingId(request.InvitingUserId, request.MeetingId);

      if (existsMeetingInvitation)
      {
        throw new ConflictException(
          $"{nameof(MeetingInvitation)}(InvitedUserId={request.InvitingUserId}, MeetingId={request.MeetingId}) already exists.");
      }

      var existsFriendRelation = await _relationsRepository
        .ExistsOneByUserIdAndRelatedUserIdAndTypeTwoWay(request.UserId, request.InvitingUserId, RelationType.Friend);

      if (!existsFriendRelation)
      {
        throw new NotFoundException(nameof(Relation), request.InvitingUserId);
      }

      var existsBlockedRelation = await _relationsRepository
        .ExistsOneByUserIdAndRelatedUserIdAndTypeTwoWay(request.UserId, request.InvitingUserId, RelationType.Blocked);

      if (existsBlockedRelation)
      {
        throw new ConflictException(
          $"{nameof(User)}({request.UserId}) is blocked/blocking {nameof(User)}({request.InvitingUserId}).");
      }
    }
  }
}
