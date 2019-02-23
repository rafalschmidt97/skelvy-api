using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Common;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;
using static Skelvy.Application.Meetings.Commands.CreateMeetingRequest.CreateMeetingRequestHelper;

namespace Skelvy.Application.Meetings.Commands.MatchMeetingRequests
{
  // TODO: handler contains same logic as CreateMeetingRequestCommandHandler
  public class MatchMeetingRequestsCommandHandler : IRequestHandler<MatchMeetingRequestsCommand>
  {
    private readonly SkelvyContext _context;

    public MatchMeetingRequestsCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(MatchMeetingRequestsCommand request, CancellationToken cancellationToken)
    {
      var requests = await _context.MeetingRequests
        .Include(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .ToListAsync(cancellationToken);
      var isDataChanged = false;

      foreach (var meetingRequest in requests)
      {
        var existingRequest = requests.FirstOrDefault(x => AreRequestsMatch(x, meetingRequest));

        if (existingRequest != null)
        {
          CreateNewMeeting(meetingRequest, existingRequest);
          isDataChanged = true;
        }
      }

      if (isDataChanged)
      {
        await _context.SaveChangesAsync(cancellationToken);
      }

      return Unit.Value;
    }

    private static bool AreRequestsMatch(
      MeetingRequest request1,
      MeetingRequest request2)
    {
      return request1.Id != request2.Id &&
             request1.Status == MeetingStatusTypes.Searching &&
             request2.Status == MeetingStatusTypes.Searching &&
             request1.MinDate <= request2.MaxDate &&
             request1.MaxDate >= request2.MinDate &&
             CalculateAge(request1.User.Profile.Birthday) >= request2.MinAge &&
             CalculateAge(request1.User.Profile.Birthday) <= request2.MaxAge &&
             CalculateAge(request2.User.Profile.Birthday) >= request1.MinAge &&
             CalculateAge(request2.User.Profile.Birthday) <= request1.MaxAge &&
             CalculateDistance(
               request1.Latitude,
               request1.Longitude,
               request2.Latitude,
               request2.Longitude) <= 5 &&
             request2.Drinks.Any(x => request1.Drinks.Any(y => y.Drink.Id == x.DrinkId));
    }

    private void CreateNewMeeting(
      MeetingRequest request1,
      MeetingRequest request2)
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
    }
  }
}
