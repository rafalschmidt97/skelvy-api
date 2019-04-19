using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;
using static Skelvy.Application.Meetings.Commands.CreateMeetingRequest.CreateMeetingRequestHelper;

namespace Skelvy.Application.Meetings.Commands.CreateMeetingRequest
{
  public class CreateMeetingRequestCommandHandler : CommandHandler<CreateMeetingRequestCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public CreateMeetingRequestCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(CreateMeetingRequestCommand request)
    {
      var data = await ValidateData(request);
      var newRequest = await CreateNewMeetingRequest(request);
      var meeting = await FindMatchingMeeting(newRequest, data.user);

      if (meeting != null)
      {
        await AddUserToMeeting(newRequest, meeting);
      }
      else
      {
        await MatchExistingMeetingRequests(newRequest, data.user);
      }

      return Unit.Value;
    }

    private async Task<dynamic> ValidateData(CreateMeetingRequestCommand request)
    {
      var user = await _context.Users
        .Include(x => x.Profile)
        .FirstOrDefaultAsync(x => x.Id == request.UserId);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      if (user.Profile == null)
      {
        throw new ConflictException(
          $"Entity {nameof(UserProfile)}({nameof(request.UserId)}={request.UserId}) must exists.");
      }

      var drinks = await _context.Drinks.ToListAsync();
      var filteredDrinks = drinks.Where(x => request.Drinks.Any(y => y.Id == x.Id)).ToList();

      if (filteredDrinks.Count != request.Drinks.Count)
      {
        throw new NotFoundException(nameof(Drink), request.Drinks);
      }

      var requestExists = _context.MeetingRequests.Any(x => x.UserId == request.UserId && !x.IsRemoved);

      if (requestExists)
      {
        throw new ConflictException(
          $"Entity {nameof(MeetingRequest)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }

      var meetingExists = _context.MeetingUsers.Any(x => x.UserId == request.UserId && !x.IsRemoved);

      if (meetingExists)
      {
        throw new ConflictException(
          $"Entity {nameof(Meeting)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }

      return new { user };
    }

    private async Task<MeetingRequest> CreateNewMeetingRequest(CreateMeetingRequestCommand request)
    {
      var meetingRequest = new MeetingRequest
      {
        Status = MeetingRequestStatusTypes.Searching,
        MinDate = request.MinDate,
        MaxDate = request.MaxDate,
        MinAge = request.MinAge,
        MaxAge = request.MaxAge,
        Latitude = request.Latitude,
        Longitude = request.Longitude,
        UserId = request.UserId,
      };

      _context.MeetingRequests.Add(meetingRequest);

      var meetingRequestDrinks = PrepareDrinks(request.Drinks, meetingRequest);
      _context.MeetingRequestDrinks.AddRange(meetingRequestDrinks);

      await _context.SaveChangesAsync();
      return meetingRequest;
    }

    private async Task<Meeting> FindMatchingMeeting(MeetingRequest newRequest, User user)
    {
      var meetings = await _context.Meetings
        .Include(x => x.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Users)
        .ThenInclude(x => x.MeetingRequest)
        .ThenInclude(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .Where(x => x.Date >= newRequest.MinDate && x.Date <= newRequest.MaxDate && !x.IsRemoved)
        .ToListAsync();

      return meetings.FirstOrDefault(x => IsMeetingMatchRequest(x, newRequest, user));
    }

    private static bool IsMeetingMatchRequest(Meeting meeting, MeetingRequest newRequest, User user)
    {
      return meeting.Users.Where(x => !x.IsRemoved)
               .All(x => IsUserAgeWithinMeetingRequestAgeRange(
                 CalculateAge(x.User.Profile.Birthday),
                 newRequest.MinAge,
                 newRequest.MaxAge)) &&
             meeting.Users.Where(x => !x.IsRemoved)
               .All(x => IsUserAgeWithinMeetingRequestAgeRange(
                 CalculateAge(user.Profile.Birthday),
                 x.MeetingRequest.MinAge,
                 x.MeetingRequest.MaxAge)) &&
             meeting.Users.Count(x => !x.IsRemoved) < 4 &&
             CalculateDistance(
               meeting.Latitude,
               meeting.Longitude,
               newRequest.Latitude,
               newRequest.Longitude) <= 5 &&
             newRequest.Drinks.Any(x => x.DrinkId == meeting.DrinkId);
    }

    private static bool IsUserAgeWithinMeetingRequestAgeRange(int age, int minAge, int maxAge)
    {
      return age >= minAge && (maxAge >= 55 || age <= maxAge);
    }

    private async Task AddUserToMeeting(MeetingRequest newRequest, Meeting meeting)
    {
      var meetingUser = new MeetingUser
      {
        MeetingId = meeting.Id,
        UserId = newRequest.UserId,
        MeetingRequestId = newRequest.Id,
      };

      _context.MeetingUsers.Add(meetingUser);
      newRequest.UpdateStatus(MeetingRequestStatusTypes.Found);

      await _context.SaveChangesAsync();
      await BroadcastUserJoinedMeeting(meetingUser);
    }

    private async Task MatchExistingMeetingRequests(MeetingRequest newRequest, User user)
    {
      var existingRequest = await FindMatchingMeetingRequest(newRequest, user);

      if (existingRequest != null)
      {
        await CreateNewMeeting(newRequest, existingRequest);
      }
    }

    private async Task<MeetingRequest> FindMatchingMeetingRequest(MeetingRequest newRequest, User user)
    {
      var requests = await _context.MeetingRequests
        .Include(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .Where(x => x.Id != newRequest.Id &&
                    x.Status == MeetingRequestStatusTypes.Searching &&
                    !x.IsRemoved &&
                    x.MinDate <= newRequest.MaxDate &&
                    x.MaxDate >= newRequest.MinDate)
        .ToListAsync();

      return requests.FirstOrDefault(x => AreRequestsMatch(x, newRequest, user));
    }

    private static bool AreRequestsMatch(
      MeetingRequest request,
      MeetingRequest newRequest,
      User user)
    {
      return IsUserAgeWithinMeetingRequestAgeRange(CalculateAge(request.User.Profile.Birthday), newRequest.MinAge, newRequest.MaxAge) &&
             IsUserAgeWithinMeetingRequestAgeRange(CalculateAge(user.Profile.Birthday), request.MinAge, request.MaxAge) &&
             CalculateDistance(
               request.Latitude,
               request.Longitude,
               newRequest.Latitude,
               newRequest.Longitude) <= 5 &&
             newRequest.Drinks.Any(x => request.Drinks.Any(y => y.Drink.Id == x.DrinkId));
    }

    private async Task CreateNewMeeting(MeetingRequest request1, MeetingRequest request2)
    {
      var meeting = new Meeting
      {
        Date = FindCommonDate(request1, request2),
        Latitude = request1.Latitude,
        Longitude = request1.Longitude,
        DrinkId = FindCommonDrink(request1, request2),
      };

      _context.Meetings.Add(meeting);

      var meetingUsers = new[]
      {
        new MeetingUser
        {
          MeetingId = meeting.Id,
          UserId = request1.UserId,
          MeetingRequestId = request1.Id,
        },
        new MeetingUser
        {
          MeetingId = meeting.Id,
          UserId = request2.UserId,
          MeetingRequestId = request2.Id,
        },
      };

      _context.MeetingUsers.AddRange(meetingUsers);

      request1.UpdateStatus(MeetingRequestStatusTypes.Found);
      request2.UpdateStatus(MeetingRequestStatusTypes.Found);

      await _context.SaveChangesAsync();
      await BroadcastUserFoundMeeting(request1, request2);
    }

    private async Task BroadcastUserFoundMeeting(MeetingRequest request1, MeetingRequest request2)
    {
      var userIds = new List<int> { request1.UserId, request2.UserId };
      await _notifications.BroadcastUserFoundMeeting(userIds);
    }

    private async Task BroadcastUserJoinedMeeting(MeetingUser meetingUser)
    {
      var meetingUsers = await _context.MeetingUsers
        .Where(x => x.MeetingId == meetingUser.MeetingId)
        .ToListAsync();

      var meetingUserIds = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserJoinedMeeting(meetingUser, meetingUserIds);
    }

    private static IEnumerable<MeetingRequestDrink> PrepareDrinks(
      IEnumerable<CreateMeetingRequestDrink> requestDrinks,
      MeetingRequest request)
    {
      return requestDrinks.Select(requestDrink => new MeetingRequestDrink
      {
        MeetingRequestId = request.Id,
        DrinkId = requestDrink.Id,
      });
    }
  }
}
