using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;
using static Skelvy.Application.Meetings.Commands.CreateMeetingRequest.CreateMeetingRequestHelper;

namespace Skelvy.Application.Meetings.Commands.CreateMeetingRequest
{
  // TODO: handler contains same logic as MatchMeetingRequestsCommandHandler
  public class CreateMeetingRequestCommandHandler : IRequestHandler<CreateMeetingRequestCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public CreateMeetingRequestCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public async Task<Unit> Handle(CreateMeetingRequestCommand request, CancellationToken cancellationToken)
    {
      var data = await ValidateData(request, cancellationToken);
      var newRequest = await CreateNewMeetingRequest(request, cancellationToken);
      var meeting = await FindMatchingMeeting(newRequest, data.user, cancellationToken);

      if (meeting != null)
      {
        await AddUserToMeeting(newRequest, meeting, cancellationToken);
      }
      else
      {
        await MatchExistingMeetingRequests(newRequest, data.user, cancellationToken);
      }

      return Unit.Value;
    }

    private async Task<dynamic> ValidateData(CreateMeetingRequestCommand request, CancellationToken cancellationToken)
    {
      var user = await _context.Users
        .Include(x => x.Profile)
        .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      if (user.Profile == null)
      {
        throw new ConflictException(
          $"Entity {nameof(UserProfile)}({nameof(request.UserId)}={request.UserId}) must exists.");
      }

      var drinks = await _context.Drinks.ToListAsync(cancellationToken);
      var filteredDrinks = drinks.Where(x => request.Drinks.Any(y => y.Id == x.Id)).ToList();

      if (filteredDrinks.Count != request.Drinks.Count)
      {
        throw new NotFoundException(nameof(Drink), request.Drinks);
      }

      var requestExists = _context.MeetingRequests.Any(x => x.UserId == request.UserId);

      if (requestExists)
      {
        throw new ConflictException(
          $"Entity {nameof(MeetingRequest)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }

      var meetingExists = _context.Meetings.Any(x => x.Users.Any(y => y.UserId == request.UserId));

      if (meetingExists)
      {
        throw new ConflictException(
          $"Entity {nameof(Meeting)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }

      return new { user };
    }

    private async Task<MeetingRequest> CreateNewMeetingRequest(
      CreateMeetingRequestCommand request,
      CancellationToken cancellationToken)
    {
      var meetingRequest = new MeetingRequest
      {
        Status = MeetingStatusTypes.Searching,
        MinDate = request.MinDate,
        MaxDate = request.MaxDate,
        MinAge = request.MinAge,
        MaxAge = request.MaxAge,
        Latitude = request.Latitude,
        Longitude = request.Longitude,
        UserId = request.UserId
      };
      _context.MeetingRequests.Add(meetingRequest);

      var meetingRequestDrinks = PrepareDrinks(request.Drinks, meetingRequest);
      _context.MeetingRequestDrinks.AddRange(meetingRequestDrinks);

      await _context.SaveChangesAsync(cancellationToken);

      return meetingRequest;
    }

    private async Task<Meeting> FindMatchingMeeting(
      MeetingRequest newRequest,
      User user,
      CancellationToken cancellationToken)
    {
      var meetings = await _context.Meetings
        .Include(x => x.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.MeetingRequest)
        .ThenInclude(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .ToListAsync(cancellationToken);

      return meetings.FirstOrDefault(x => IsMeetingMatchRequest(x, newRequest, user));
    }

    private static bool IsMeetingMatchRequest(Meeting meeting, MeetingRequest newRequest, User user)
    {
      return meeting.Date >= newRequest.MinDate && meeting.Date <= newRequest.MaxDate &&
             meeting.Users.All(x => IsUserAgeWithinMeetingRequestAgeRange(CalculateAge(x.User.Profile.Birthday), newRequest.MinAge, newRequest.MaxAge)) &&
             meeting.Users.All(x => IsUserAgeWithinMeetingRequestAgeRange(CalculateAge(user.Profile.Birthday), x.User.MeetingRequest.MinAge, x.User.MeetingRequest.MaxAge)) &&
             meeting.Users.Count < 4 &&
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

    private async Task AddUserToMeeting(
      MeetingRequest newRequest,
      Meeting meeting,
      CancellationToken cancellationToken)
    {
      var meetingUser = new MeetingUser
      {
        MeetingId = meeting.Id,
        UserId = newRequest.UserId
      };

      _context.MeetingUsers.Add(meetingUser);

      newRequest.Status = MeetingStatusTypes.Found;

      await _context.SaveChangesAsync(cancellationToken);
      await BroadcastUserJoinedMeeting(meetingUser, cancellationToken);
    }

    private async Task MatchExistingMeetingRequests(
      MeetingRequest newRequest,
      User user,
      CancellationToken cancellationToken)
    {
      var existingRequest = await FindMatchingMeetingRequest(newRequest, user, cancellationToken);

      if (existingRequest != null)
      {
        await CreateNewMeeting(newRequest, existingRequest, cancellationToken);
      }
    }

    private async Task<MeetingRequest> FindMatchingMeetingRequest(
      MeetingRequest newRequest,
      User user,
      CancellationToken cancellationToken)
    {
      var requests = await _context.MeetingRequests
        .Include(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .ToListAsync(cancellationToken);

      return requests.FirstOrDefault(x => AreRequestsMatch(x, newRequest, user));
    }

    private static bool AreRequestsMatch(
      MeetingRequest request,
      MeetingRequest newRequest,
      User user)
    {
      return request.Id != newRequest.Id &&
             request.Status == MeetingStatusTypes.Searching &&
             request.MinDate <= newRequest.MaxDate &&
             request.MaxDate >= newRequest.MinDate &&
             IsUserAgeWithinMeetingRequestAgeRange(CalculateAge(request.User.Profile.Birthday), newRequest.MinAge, newRequest.MaxAge) &&
             IsUserAgeWithinMeetingRequestAgeRange(CalculateAge(user.Profile.Birthday), request.MinAge, request.MaxAge) &&
             CalculateDistance(
               request.Latitude,
               request.Longitude,
               newRequest.Latitude,
               newRequest.Longitude) <= 5 &&
             newRequest.Drinks.Any(x => request.Drinks.Any(y => y.Drink.Id == x.DrinkId));
    }

    private async Task CreateNewMeeting(
      MeetingRequest request1,
      MeetingRequest request2,
      CancellationToken cancellationToken)
    {
      var meeting = new Meeting
      {
        Date = FindCommonDate(request1, request2),
        Latitude = request1.Latitude,
        Longitude = request1.Longitude,
        DrinkId = FindCommonDrink(request1, request2)
      };

      _context.Meetings.Add(meeting);

      var meetingUsers = new[]
      {
        new MeetingUser
        {
          MeetingId = meeting.Id,
          UserId = request1.UserId
        },
        new MeetingUser
        {
          MeetingId = meeting.Id,
          UserId = request2.UserId
        }
      };

      _context.MeetingUsers.AddRange(meetingUsers);

      request1.Status = MeetingStatusTypes.Found;
      request2.Status = MeetingStatusTypes.Found;

      await _context.SaveChangesAsync(cancellationToken);
      await BroadcastUserFoundMeeting(request1, request2, cancellationToken);
    }

    private async Task BroadcastUserFoundMeeting(MeetingRequest request1, MeetingRequest request2, CancellationToken cancellationToken)
    {
      var userIds = new List<int> { request1.UserId, request2.UserId };
      await _notifications.BroadcastUserFoundMeeting(userIds, cancellationToken);
    }

    private async Task BroadcastUserJoinedMeeting(MeetingUser meetingUser, CancellationToken cancellationToken)
    {
      var meetingUsers = await _context.MeetingUsers
        .Where(x => x.MeetingId == meetingUser.MeetingId)
        .ToListAsync(cancellationToken);

      var meetingUserIds = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserJoinedMeeting(meetingUser, meetingUserIds, cancellationToken);
    }

    private static IEnumerable<MeetingRequestDrink> PrepareDrinks(
      IEnumerable<CreateMeetingRequestDrink> requestDrinks,
      MeetingRequest request)
    {
      return requestDrinks.Select(requestDrink => new MeetingRequestDrink
      {
        MeetingRequestId = request.Id,
        DrinkId = requestDrink.Id
      });
    }
  }
}
