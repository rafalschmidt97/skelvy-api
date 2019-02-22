using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Common;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.CreateMeetingRequest
{
  public class CreateMeetingRequestCommandHandler : IRequestHandler<CreateMeetingRequestCommand>
  {
    private readonly SkelvyContext _context;

    public CreateMeetingRequestCommandHandler(SkelvyContext context)
    {
      _context = context;
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
        var existingRequest = await FindMatchingMeetingRequest(newRequest, data.user, cancellationToken);

        if (existingRequest != null)
        {
          await CreateNewMeeting(newRequest, existingRequest, cancellationToken);
        }
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

      var drinks = await _context.Drinks.ToListAsync(cancellationToken);
      var filteredDrinks = drinks.FindAll(x => request.Drinks.Any(y => y.Id == x.Id));

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
        MinDate = request.MinDate.Date,
        MaxDate = request.MaxDate.Date,
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

      return meetings.Find(x => IsMeetingMatchRequest(x, newRequest, user));
    }

    private static bool IsMeetingMatchRequest(Meeting meeting, MeetingRequest newRequest, User user)
    {
      return meeting.Date >= newRequest.MinDate && meeting.Date <= newRequest.MaxDate &&
             meeting.Users.All(x => CalculateAge(x.User.Profile.Birthday) >= newRequest.MinAge &&
                                    CalculateAge(x.User.Profile.Birthday) <= newRequest.MaxAge) &&
             meeting.Users.All(x => CalculateAge(user.Profile.Birthday) >= x.User.MeetingRequest.MinAge &&
                                    CalculateAge(user.Profile.Birthday) <= x.User.MeetingRequest.MaxAge) &&
             meeting.Users.Count < 4 &&
             CalculateDistance(
               meeting.Latitude,
               meeting.Longitude,
               newRequest.Latitude,
               newRequest.Longitude) <= 5 &&
             newRequest.Drinks.Any(x => x.DrinkId == meeting.DrinkId);
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

      return requests.Find(x => AreRequestsMatch(x, newRequest, user));
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
             CalculateAge(request.User.Profile.Birthday) >= newRequest.MinAge &&
             CalculateAge(request.User.Profile.Birthday) <= newRequest.MaxAge &&
             CalculateAge(user.Profile.Birthday) >= request.MinAge &&
             CalculateAge(user.Profile.Birthday) <= request.MaxAge &&
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

    private static double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
    {
      var d1 = latitude1 * (Math.PI / 180.0);
      var num1 = longitude1 * (Math.PI / 180.0);
      var d2 = latitude2 * (Math.PI / 180.0);
      var num2 = longitude2 * (Math.PI / 180.0) - num1;
      var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
               Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

      return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))) / 1000;
    }

    private static int CalculateAge(DateTime date)
    {
      var age = DateTime.Now.Year - date.Year;
      if (DateTime.Now.DayOfYear < date.DayOfYear)
      {
        age = age - 1;
      }

      return age;
    }

    private static DateTime FindCommonDate(MeetingRequest request1, MeetingRequest request2)
    {
      var dates = new List<DateTime>();

      for (var i = request1.MinDate; i <= request1.MaxDate; i = i.AddDays(1))
      {
        dates.Add(i);
      }

      var commonDates = new List<DateTime>();

      foreach (var date in dates)
      {
        if (date >= request1.MinDate && date < request1.MaxDate &&
              date >= request2.MinDate && date < request2.MaxDate)
        {
          commonDates.Add(date);
        }
      }

      return dates.First();
    }

    private static int FindCommonDrink(MeetingRequest request1, MeetingRequest request2)
    {
      return request1.Drinks.First(x => request2.Drinks.Any(y => y.DrinkId == x.DrinkId)).DrinkId;
    }
  }
}
