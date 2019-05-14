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
        .ThenInclude(y => y.Profile)
        .Include(x => x.Drink)
        .FirstOrDefaultAsync(x => x.Users.Any(y => y.UserId == userId && !y.IsRemoved) && !x.IsRemoved);

      if (meeting != null)
      {
        var users = meeting.Users.Where(x => !x.IsRemoved).ToList();

        foreach (var user in users)
        {
          var userPhotos = await Context.UserProfilePhotos
            .Where(x => x.ProfileId == user.User.Profile.Id)
            .OrderBy(x => x.Order)
            .ToListAsync();

          user.User.Profile.Photos = userPhotos;
        }

        meeting.Users = users;
      }

      return meeting;
    }

    public async Task<IList<Meeting>> FindAllAfterDate(DateTimeOffset maxDate)
    {
      return await Context.Meetings
        .Where(x => !x.IsRemoved && x.Date < maxDate)
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
        .Where(x => !x.IsRemoved &&
                    x.Date >= request.MinDate &&
                    x.Date <= request.MaxDate &&
                    x.Users.Count(y => !y.IsRemoved) < 4)
        .ToListAsync();

      return meetings.FirstOrDefault(x => IsMeetingMatchRequest(x, request, user));
    }

    public async Task Add(Meeting meeting)
    {
      await Context.Meetings.AddAsync(meeting);
      await SaveChanges();
    }

    public async Task Update(Meeting meeting)
    {
      Context.Meetings.Update(meeting);
      await SaveChanges();
    }

    public async Task UpdateRange(IList<Meeting> meetings)
    {
      Context.Meetings.UpdateRange(meetings);
      await SaveChanges();
    }

    private static bool IsMeetingMatchRequest(Meeting meeting, MeetingRequest request, User requestUser)
    {
      return meeting.Users.Where(x => !x.IsRemoved).All(x => x.User.Profile.IsWithinMeetingRequestAgeRange(request)) &&
             meeting.Users.Where(x => !x.IsRemoved).All(x => requestUser.Profile.IsWithinMeetingRequestAgeRange(x.MeetingRequest)) &&
             meeting.GetDistance(request) <= 5;
    }
  }
}
