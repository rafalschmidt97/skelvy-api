using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;
using Skelvy.Domain.Entities;
using static Skelvy.Application.Meetings.Commands.CreateMeetingRequest.CreateMeetingRequestHelper;

namespace Skelvy.Application.Meetings.Commands.MatchMeetingRequests
{
  public class MatchMeetingRequestsCommandHandler : CommandHandler<MatchMeetingRequestsCommand>
  {
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly INotificationsService _notifications;

    public MatchMeetingRequestsCommandHandler(IMeetingRequestsRepository meetingRequestsRepository, INotificationsService notifications)
    {
      _meetingRequestsRepository = meetingRequestsRepository;
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
        await _meetingRequestsRepository.Context.SaveChangesAsync();
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
             IsUserAgeWithinMeetingRequestAgeRange(CalculateAge(request1.User.Profile.Birthday), request2.MinAge, request2.MaxAge) &&
             IsUserAgeWithinMeetingRequestAgeRange(CalculateAge(request2.User.Profile.Birthday), request1.MinAge, request1.MaxAge) &&
             CalculateDistance(
               request1.Latitude,
               request1.Longitude,
               request2.Latitude,
               request2.Longitude) <= 5 &&
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
        FindCommonDate(request1, request2),
        request1.Latitude,
        request1.Longitude,
        FindCommonDrink(request1, request2));

      _meetingRequestsRepository.Context.Meetings.Add(meeting);

      var meetingUsers = new[]
      {
        new MeetingUser(meeting.Id, request1.UserId, request1.Id),
        new MeetingUser(meeting.Id, request2.UserId, request2.Id),
      };

      _meetingRequestsRepository.Context.MeetingUsers.AddRange(meetingUsers);

      request1.MarkAsFound();
      request2.MarkAsFound();
    }

    private async Task BroadcastUserFoundMeeting(IEnumerable<MeetingRequest> updatedRequests)
    {
      var userIds = updatedRequests.Select(x => x.User.Id).ToList();
      await _notifications.BroadcastUserFoundMeeting(userIds);
    }
  }
}
