using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;
using static Skelvy.Application.Meetings.Commands.CreateMeetingRequest.CreateMeetingRequestHelper;

namespace Skelvy.Application.Core.Persistence
{
  public class MeetingsRepository : BaseRepository, IMeetingsRepository
  {
    public MeetingsRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<Meeting> FindOneWithUsersDetailsAndDrinkByUserId(int userId)
    {
      return await Context.Meetings
        .Include(x => x.Users)
        .ThenInclude(y => y.User)
        .ThenInclude(x => x.Profile)
        .ThenInclude(x => x.Photos)
        .Include(x => x.Drink)
        .FirstOrDefaultAsync(x => x.Users.Any(y => y.UserId == userId && !y.IsRemoved));
    }

    public async Task<IList<Meeting>> FindAllAfterDate(DateTimeOffset maxDate)
    {
      return await Context.Meetings
        .Where(x => x.Date < maxDate && !x.IsRemoved)
        .ToListAsync();
    }

    public async Task<Meeting> FindOneMatchingUserRequest(User user, MeetingRequest request)
    {
      var meetings = await Context.Meetings
        .Include(x => x.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Users)
        .ThenInclude(x => x.MeetingRequest)
        .ThenInclude(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .Where(x => x.Date >= request.MinDate && x.Date <= request.MaxDate && !x.IsRemoved)
        .ToListAsync();

      return meetings.FirstOrDefault(x => IsMeetingMatchRequest(x, request, user));
    }

    private static bool IsMeetingMatchRequest(Meeting meeting, MeetingRequest request, User requestUser)
    {
      return meeting.Users.Where(x => !x.IsRemoved)
               .All(x => IsUserAgeWithinMeetingRequestAgeRange(
                 CalculateAge(x.User.Profile.Birthday),
                 request.MinAge,
                 request.MaxAge)) &&
             meeting.Users.Where(x => !x.IsRemoved)
               .All(x => IsUserAgeWithinMeetingRequestAgeRange(
                 CalculateAge(requestUser.Profile.Birthday),
                 x.MeetingRequest.MinAge,
                 x.MeetingRequest.MaxAge)) &&
             meeting.Users.Count(x => !x.IsRemoved) < 4 &&
             CalculateDistance(
               meeting.Latitude,
               meeting.Longitude,
               request.Latitude,
               request.Longitude) <= 5 &&
             request.Drinks.Any(x => x.DrinkId == meeting.DrinkId);
    }

    private static bool IsUserAgeWithinMeetingRequestAgeRange(int age, int minAge, int maxAge)
    {
      return age >= minAge && (maxAge >= 55 || age <= maxAge);
    }
  }
}
