using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Extensions;

namespace Skelvy.Persistence.Repositories
{
  public class MeetingsRepository : BaseRepository, IMeetingsRepository
  {
    public MeetingsRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<Meeting> FindOneWithUsersDetailsAndDrinkByUserId(int userId)
    {
      var meeting = await Context.Meetings
        .Include(x => x.Users)
        .ThenInclude(y => y.User)
        .ThenInclude(x => x.Profile)
        .ThenInclude(x => x.Photos)
        .Include(x => x.Drink)
        .FirstOrDefaultAsync(x => x.Users.Any(y => y.UserId == userId && !y.IsRemoved));

      return new Meeting(
        meeting.Id,
        meeting.Date,
        meeting.Latitude,
        meeting.Longitude,
        meeting.CreatedAt,
        meeting.IsRemoved,
        meeting.RemovedAt,
        meeting.RemovedReason,
        meeting.DrinkId,
        meeting.Users.Where(x => !x.IsRemoved).ToList(),
        meeting.ChatMessages,
        meeting.Drink);
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

    public void UpdateAsTransaction(Meeting meeting)
    {
      Context.Meetings.Update(meeting);
    }

    public void AddAsTransaction(Meeting meeting)
    {
      Context.Meetings.Add(meeting);
    }

    public void UpdateRangeAsTransaction(IList<Meeting> meetings)
    {
      Context.Meetings.UpdateRange(meetings);
    }

    private static bool IsMeetingMatchRequest(Meeting meeting, MeetingRequest request, User requestUser)
    {
      return meeting.Users.Where(x => !x.IsRemoved).All(x => x.User.Profile.IsWithinMeetingRequestAgeRange(request)) &&
             meeting.Users.Where(x => !x.IsRemoved).All(x => requestUser.Profile.IsWithinMeetingRequestAgeRange(x.MeetingRequest)) &&
             meeting.Users.Count(x => !x.IsRemoved) < 4 &&
             meeting.GetDistance(request) <= 5 &&
             request.Drinks.Any(x => x.DrinkId == meeting.DrinkId);
    }
  }
}
