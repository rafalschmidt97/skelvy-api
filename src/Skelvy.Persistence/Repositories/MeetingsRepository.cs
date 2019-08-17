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

    public async Task<bool> ExistsOne(int id)
    {
      return await Context.Meetings
        .AnyAsync(x => x.Id == id && !x.IsRemoved);
    }

    public async Task<bool> ExistsOneByGroupId(int groupId)
    {
      return await Context.Meetings
        .AnyAsync(x => x.GroupId == groupId && !x.IsRemoved);
    }

    public async Task<Meeting> FindOne(int id)
    {
      return await Context.Meetings
        .FirstOrDefaultAsync(x => x.Id == id && !x.IsRemoved);
    }

    public async Task<IList<Meeting>> FindAllWithActivityByUserId(int userId)
    {
      return await Context.Meetings
        .Include(x => x.Group)
        .ThenInclude(y => y.Users)
        .Include(x => x.Activity)
        .Where(x => x.Group.Users.Any(y => y.UserId == userId && !y.IsRemoved) && !x.IsRemoved)
        .ToListAsync();
    }

    public async Task<Meeting> FindOneByGroupId(int groupId)
    {
      return await Context.Meetings
        .Include(x => x.Group)
        .FirstOrDefaultAsync(x => x.GroupId == groupId && !x.IsRemoved && !x.Group.IsRemoved);
    }

    public async Task<Meeting> FindOneWithGroupByGroupId(int groupId)
    {
      return await Context.Meetings
        .Include(x => x.Group)
        .FirstOrDefaultAsync(x => x.GroupId == groupId && !x.IsRemoved && !x.Group.IsRemoved);
    }

    public async Task<IList<Meeting>> FindAllAfterOrEqualDate(DateTimeOffset maxDate)
    {
      return await Context.Meetings
        .Where(x => !x.IsRemoved && x.Date <= maxDate)
        .ToListAsync();
    }

    public async Task<IList<Meeting>> FindAllNonHiddenCloseWithUsersDetailsByUserIdAndLocation(int userId, double latitude, double longitude)
    {
      var user = await Context.Users
        .Include(x => x.Profile)
        .FirstOrDefaultAsync(x => x.Id == userId && !x.IsRemoved);

      var meetings = await Context.Meetings
        .Include(x => x.Group)
        .ThenInclude(x => x.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Group)
        .ThenInclude(x => x.Users)
        .ThenInclude(x => x.MeetingRequest)
        .Include(x => x.Activity)
        .Where(x => !x.IsRemoved &&
                    !x.IsHidden &&
                    x.Group.Users.Count(y => !y.IsRemoved) < x.Activity.Size)
        .ToListAsync();

      if (meetings.Any())
      {
        var matchingMeetings = meetings.Where(x => IsMeetingClose(x, user, latitude, longitude)).ToList();
        matchingMeetings.ForEach(x => x.Group.Users = x.Group.Users.Where(y => !y.IsRemoved).ToList());
        foreach (var meeting in matchingMeetings)
        {
          foreach (var groupUser in meeting.Group.Users)
          {
            var userPhotos = await Context.ProfilePhotos
              .Include(x => x.Attachment)
              .Where(x => x.ProfileId == groupUser.User.Profile.Id)
              .OrderBy(x => x.Order)
              .ToListAsync();

            groupUser.User.Profile.Photos = userPhotos;
          }
        }

        return matchingMeetings;
      }

      return new List<Meeting>();
    }

    public async Task<Meeting> FindOneNonHiddenAndNonFullByMeetingIdAndUserId(int meetingId, int userId)
    {
      return await Context.Meetings
        .Include(x => x.Group)
        .ThenInclude(x => x.Users)
        .FirstOrDefaultAsync(x => x.Id == meetingId &&
                                  !x.IsHidden &&
                                  x.Group.Users.Count(y => !y.IsRemoved) < x.Size &&
                                  !x.IsRemoved);
    }

    public async Task<Meeting> FindOneNonFullByMeetingId(int meetingId)
    {
      return await Context.Meetings
        .Include(x => x.Group)
        .ThenInclude(x => x.Users)
        .FirstOrDefaultAsync(x => x.Id == meetingId &&
                                  x.Group.Users.Count(y => !y.IsRemoved) < x.Size &&
                                  !x.IsRemoved);
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

    private static bool IsMeetingClose(Meeting meeting, User user, double latitude, double longitude)
    {
      return meeting.Group.Users.Where(x => !x.IsRemoved).All(x => x.MeetingRequest == null || user.Profile.IsWithinMeetingRequestAgeRange(x.MeetingRequest)) &&
             meeting.GetDistance(latitude, longitude) <= meeting.Activity.Distance;
    }
  }
}
