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
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMediator _mediator;

    public LeaveMeetingCommandHandler(
      IMeetingUsersRepository meetingUsersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMediator mediator)
    {
      _meetingUsersRepository = meetingUsersRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(LeaveMeetingCommand request)
    {
      var meetingUser = await _meetingUsersRepository.FindOneWithMeetingByUserId(request.UserId);

      if (meetingUser == null)
      {
        throw new NotFoundException(nameof(MeetingUser), request.UserId);
      }

      var meetingUsers = await _meetingUsersRepository.FindAllWithMeetingRequestByMeetingId(meetingUser.MeetingId);
      var userDetails = meetingUsers.First(x => x.UserId == meetingUser.UserId);

      if (userDetails.MeetingRequest.IsSearching)
      {
        throw new InternalServerErrorException(
          $"Entity {nameof(MeetingRequest)}(UserId = {request.UserId}) is marked as '{MeetingRequestStatusTypes.Searching}' " +
          $"while {nameof(MeetingUser)} exists");
      }

      using (var transaction = _meetingsRepository.BeginTransaction())
      {
        userDetails.Leave();
        userDetails.MeetingRequest.Abort();

        await _meetingUsersRepository.Update(userDetails);
        await _meetingRequestsRepository.Update(userDetails.MeetingRequest);

        var meetingAborted = false;

        if (meetingUsers.Count == 2)
        {
          var anotherUserDetails = meetingUsers.First(x => x.UserId != meetingUser.UserId);

          anotherUserDetails.Abort();
          anotherUserDetails.MeetingRequest.MarkAsSearching();
          meetingUser.Meeting.Abort();

          await _meetingUsersRepository.Update(anotherUserDetails);
          await _meetingRequestsRepository.Update(anotherUserDetails.MeetingRequest);
          await _meetingsRepository.Update(meetingUser.Meeting);

          meetingAborted = true;
        }

        transaction.Commit();

        if (!meetingAborted)
        {
          await _mediator.Publish(new UserLeftMeetingEvent(meetingUser.UserId, meetingUser.MeetingId));
        }
        else
        {
          if (userDetails.ModifiedAt != null)
          {
            await _mediator.Publish(
              new MeetingAbortedEvent(meetingUser.UserId, meetingUser.MeetingId, userDetails.ModifiedAt.Value));
          }
          else
          {
            throw new InternalServerErrorException(
              $"Entity {nameof(MeetingUser)}(UserId = {meetingUser.UserId}) has modified date null after leaving");
          }
        }
      }

      return Unit.Value;
    }
  }
}
