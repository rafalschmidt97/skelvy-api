using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;
using Skelvy.Common.Extensions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.RemoveExpiredMeetings
{
  public class RemoveExpiredMeetingsCommandHandler : CommandHandler<RemoveExpiredMeetingsCommand>
  {
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly INotificationsService _notifications;

    public RemoveExpiredMeetingsCommandHandler(
      IMeetingsRepository meetingsRepository,
      IMeetingUsersRepository meetingUsersRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      INotificationsService notifications)
    {
      _meetingsRepository = meetingsRepository;
      _meetingUsersRepository = meetingUsersRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(RemoveExpiredMeetingsCommand request)
    {
      var today = DateTimeOffset.UtcNow;
      var meetingsToRemove = await _meetingsRepository.FindAllAfterDate(today);

      var isDataChanged = false;

      if (meetingsToRemove.Count != 0)
      {
        var meetingsId = meetingsToRemove.Select(x => x.Id);
        var meetingUsers = await _meetingUsersRepository.FindAllWithMeetingRequestByMeetingsId(meetingsId);

        meetingsToRemove.ForEach(x => x.Expire());
        var meetingUsersRequest = meetingUsers.Select(x => x.MeetingRequest).ToList();
        meetingUsersRequest.ForEach(x => x.Expire());
        _meetingRequestsRepository.UpdateRangeAsTransaction(meetingUsersRequest);

        isDataChanged = true;
      }

      if (isDataChanged)
      {
        _meetingsRepository.UpdateRangeAsTransaction(meetingsToRemove);
        await _meetingsRepository.Commit();
        await BroadcastMeetingExpired(meetingsToRemove);
      }

      return Unit.Value;
    }

    private async Task BroadcastMeetingExpired(IEnumerable<Meeting> meetingsToRemove)
    {
      foreach (var meeting in meetingsToRemove)
      {
        var usersId = meeting.Users.Select(x => x.UserId).ToList();
        await _notifications.BroadcastMeetingExpired(usersId);
      }
    }
  }
}
