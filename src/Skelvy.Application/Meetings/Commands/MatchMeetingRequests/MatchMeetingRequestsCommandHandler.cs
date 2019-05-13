using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Extensions;

namespace Skelvy.Application.Meetings.Commands.MatchMeetingRequests
{
  public class MatchMeetingRequestsCommandHandler : CommandHandler<MatchMeetingRequestsCommand>
  {
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly INotificationsService _notifications;
    private readonly ILogger<MatchMeetingRequestsCommandHandler> _logger;

    public MatchMeetingRequestsCommandHandler(
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingUsersRepository meetingUsersRepository,
      INotificationsService notifications,
      ILogger<MatchMeetingRequestsCommandHandler> logger)
    {
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingsRepository = meetingsRepository;
      _meetingUsersRepository = meetingUsersRepository;
      _notifications = notifications;
      _logger = logger;
    }

    public override async Task<Unit> Handle(MatchMeetingRequestsCommand request)
    {
      var requests = await _meetingRequestsRepository.FindAllSearchingWithUsersDetailsAndDrinks();

      foreach (var meetingRequest in requests)
      {
        var existingRequest = requests.FirstOrDefault(x => AreRequestsMatch(x, meetingRequest));

        if (existingRequest != null)
        {
          await CreateNewMeeting(meetingRequest, existingRequest);
        }
      }

      return Unit.Value;
    }

    private static bool AreRequestsMatch(
      MeetingRequest request1,
      MeetingRequest request2)
    {
      return request1.Id != request2.Id &&
             request1.MinDate <= request2.MaxDate &&
             request1.MaxDate >= request2.MinDate &&
             IsUserAgeWithinMeetingRequestAgeRange(request1.User.Profile.GetAge(), request2.MinAge, request2.MaxAge) &&
             IsUserAgeWithinMeetingRequestAgeRange(request2.User.Profile.GetAge(), request1.MinAge, request1.MaxAge) &&
             request1.GetDistance(request2) <= 5 &&
             request2.Drinks.Any(x => request1.Drinks.Any(y => y.Drink.Id == x.DrinkId));
    }

    private static bool IsUserAgeWithinMeetingRequestAgeRange(int age, int minAge, int maxAge)
    {
      return age >= minAge && (maxAge >= 55 || age <= maxAge);
    }

    private async Task CreateNewMeeting(MeetingRequest request1, MeetingRequest request2) // Same logic as CreateMeetingRequest
    {
      using (var transaction = _meetingRequestsRepository.BeginTransaction())
      {
        try
        {
          var meeting = new Meeting(
            request1.FindRequiredCommonDate(request2),
            request1.Latitude,
            request1.Longitude,
            request1.FindRequiredCommonDrinkId(request2));

          await _meetingsRepository.Add(meeting);
          var meetingUsers = new[]
          {
            new MeetingUser(meeting.Id, request1.UserId, request1.Id),
            new MeetingUser(meeting.Id, request2.UserId, request2.Id),
          };

          await _meetingUsersRepository.AddRange(meetingUsers);

          request1.MarkAsFound();
          request2.MarkAsFound();
          await _meetingRequestsRepository.Update(request1);
          await _meetingRequestsRepository.Update(request2);

          transaction.Commit();
          await BroadcastUserFoundMeeting(request1, request2);
        }
        catch (Exception exception)
        {
          if (exception is DbUpdateConcurrencyException)
          {
            _logger.LogError(
              $"{nameof(MatchMeetingRequestsCommand)} Concurrency Exception for " +
              $"Requests(Id={request1.Id}, Id={request2.Id})");
          }
          else
          {
            _logger.LogError(
              $"{nameof(MatchMeetingRequestsCommand)} Exception for " +
              $"Requests(Id={request1.Id}, Id={request2.Id}): {exception.Message}");
          }
        }
      }
    }

    private async Task BroadcastUserFoundMeeting(MeetingRequest request1, MeetingRequest request2)
    {
      var usersId = new List<int> { request1.UserId, request2.UserId };
      await _notifications.BroadcastUserFoundMeeting(usersId);
    }
  }
}
