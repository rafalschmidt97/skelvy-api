using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;
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
    private readonly INotificationsService _notifications;

    public LeaveMeetingCommandHandler(
      IMeetingUsersRepository meetingUsersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      INotificationsService notifications)
    {
      _meetingUsersRepository = meetingUsersRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _notifications = notifications;
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

      userDetails.Leave();
      userDetails.MeetingRequest.Abort();

      _meetingUsersRepository.UpdateAsTransaction(userDetails);
      _meetingRequestsRepository.UpdateAsTransaction(userDetails.MeetingRequest);

      if (meetingUsers.Count == 2)
      {
        var anotherUserDetails = meetingUsers.First(x => x.UserId != meetingUser.UserId);

        anotherUserDetails.Leave();
        anotherUserDetails.MeetingRequest.MarkAsSearching();
        meetingUser.Meeting.Abort();

        _meetingUsersRepository.UpdateAsTransaction(anotherUserDetails);
        _meetingRequestsRepository.UpdateAsTransaction(anotherUserDetails.MeetingRequest);
        _meetingsRepository.UpdateAsTransaction(meetingUser.Meeting);
      }

      await _meetingUsersRepository.Commit();
      await BroadcastUserLeftMeeting(meetingUser, meetingUsers);

      return Unit.Value;
    }

    private async Task BroadcastUserLeftMeeting(MeetingUser meetingUser, IEnumerable<MeetingUser> meetingUsers)
    {
      var meetingUsersId = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserLeftMeeting(meetingUser, meetingUsersId);
    }
  }
}
