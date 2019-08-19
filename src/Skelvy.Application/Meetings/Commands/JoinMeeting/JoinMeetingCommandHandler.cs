using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.UserJoinedMeeting;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.JoinMeeting
{
  public class JoinMeetingCommandHandler : CommandHandler<JoinMeetingCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMediator _mediator;

    public JoinMeetingCommandHandler(
      IUsersRepository usersRepository,
      IMeetingsRepository meetingsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMediator mediator)
    {
      _usersRepository = usersRepository;
      _meetingsRepository = meetingsRepository;
      _groupUsersRepository = groupUsersRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(JoinMeetingCommand request)
    {
      var meeting = await ValidateData(request);

      var groupUser = new GroupUser(meeting.GroupId, request.UserId);
      await _groupUsersRepository.Add(groupUser);
      await _mediator.Publish(new UserJoinedMeetingEvent(groupUser.UserId, groupUser.GroupId));

      return Unit.Value;
    }

    private async Task<Meeting> ValidateData(JoinMeetingCommand request)
    {
      var existsUser = await _usersRepository.ExistsOne(request.UserId);

      if (!existsUser)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var meeting = await _meetingsRepository
        .FindOneNonHiddenAndNonFullByMeetingIdAndUserId(request.MeetingId, request.UserId);

      if (meeting == null)
      {
        throw new NotFoundException(nameof(Meeting), request.MeetingId);
      }

      var existsGroupUser = await _groupUsersRepository.ExistsOneByUserIdAndGroupId(request.UserId, meeting.GroupId);

      if (existsGroupUser)
      {
        throw new ConflictException(
          $"Entity {nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {meeting.GroupId}) already joined.");
      }

      return meeting;
    }
  }
}
