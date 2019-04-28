using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
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

    public MatchMeetingRequestsCommandHandler(
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingUsersRepository meetingUsersRepository,
      INotificationsService notifications)
    {
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingsRepository = meetingsRepository;
      _meetingUsersRepository = meetingUsersRepository;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(MatchMeetingRequestsCommand request)
    {
      var requests = await _meetingRequestsRepository.FindAllSearchingWithUsersDetailsAndDrinks();

      var isDataChanged = false;
      var updatedRequests = new List<MeetingRequest>();

      foreach (var meetingRequest in requests)
      {
        var existingRequest = requests.FirstOrDefault(x => AreRequestsMatch(x, meetingRequest));

        if (existingRequest != null)
        {
          CreateNewMeeting(meetingRequest, existingRequest);
          updatedRequests.Add(meetingRequest);
          updatedRequests.Add(existingRequest);
          isDataChanged = true;
        }
      }

      if (isDataChanged)
      {
        await _meetingRequestsRepository.Commit();
        await BroadcastUserFoundMeeting(updatedRequests);
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

    private void CreateNewMeeting(
      MeetingRequest request1,
      MeetingRequest request2)
    {
      var meeting = new Meeting(
        request1.FindCommonDate(request2),
        request1.Latitude,
        request1.Longitude,
        request1.FindCommonDrinkId(request2));

      _meetingsRepository.AddAsTransaction(meeting);

      var meetingUsers = new[]
      {
        new MeetingUser(meeting.Id, request1.UserId, request1.Id),
        new MeetingUser(meeting.Id, request2.UserId, request2.Id),
      };

      _meetingUsersRepository.AddRangeAsTransaction(meetingUsers);

      request1.MarkAsFound();
      request2.MarkAsFound();

      _meetingRequestsRepository.UpdateAsTransaction(request1);
      _meetingRequestsRepository.UpdateAsTransaction(request2);
    }

    private async Task BroadcastUserFoundMeeting(IEnumerable<MeetingRequest> updatedRequests)
    {
      var usersId = updatedRequests.Select(x => x.User.Id).ToList();
      await _notifications.BroadcastUserFoundMeeting(usersId);
    }
  }
}
