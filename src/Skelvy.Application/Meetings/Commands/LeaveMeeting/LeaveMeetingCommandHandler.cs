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
    private readonly INotificationsService _notifications;

    public LeaveMeetingCommandHandler(IMeetingUsersRepository meetingUsersRepository, INotificationsService notifications)
    {
      _meetingUsersRepository = meetingUsersRepository;
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

      if (meetingUsers.Count == 2)
      {
        var anotherUserDetails = meetingUsers.First(x => x.UserId != meetingUser.UserId);

        anotherUserDetails.Leave();
        anotherUserDetails.MeetingRequest.MarkAsSearching();
        meetingUser.Meeting.Abort();

        await _meetingUsersRepository.Context.SaveChangesAsync();
        await BroadcastUserLeftMeeting(meetingUser, meetingUsers);
      }

      await _meetingUsersRepository.Context.SaveChangesAsync();
      await BroadcastUserLeftMeeting(meetingUser, meetingUsers);

      return Unit.Value;
    }

    private async Task BroadcastUserLeftMeeting(MeetingUser meetingUser, IEnumerable<MeetingUser> meetingUsers)
    {
      var meetingUserIds = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserLeftMeeting(meetingUser, meetingUserIds);
    }
  }
}
