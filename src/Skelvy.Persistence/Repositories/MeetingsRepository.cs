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

    public async Task<Meeting> FindOneWithUsersDetailsAndDrinkByUserId(int userId)
    {
      var meeting = await Context.Meetings
        .Include(x => x.Users)
        .ThenInclude(y => y.User)
        .ThenInclude(y => y.Profile)
        .Include(x => x.DrinkType)
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

    public async Task<IList<Meeting>> FindAllAfterOrEqualDate(DateTimeOffset maxDate)
    {
      return await Context.Meetings
        .Where(x => !x.IsRemoved && x.Date <= maxDate)
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
        .ThenInclude(x => x.DrinkTypes)
        .ThenInclude(x => x.DrinkType)
        .Where(x => !x.IsRemoved &&
                    x.Date >= request.MinDate &&
                    x.Date <= request.MaxDate &&
                    x.Users.Count(y => !y.IsRemoved) < 4)
        .ToListAsync();

      if (meetings.Count > 0)
      {
        var blockedUsers = await Context.BlockedUsers
          .Where(x => x.UserId == user.Id && !x.IsRemoved)
          .ToListAsync();

        var filteredMeetings = new List<Meeting>();

        if (blockedUsers.Count > 0)
        {
          foreach (var meeting in meetings)
          {
            var usersId = meeting.Users.Where(x => !x.IsRemoved).Select(x => x.UserId).ToList();
            var filteredUsersId = usersId.Where(x => blockedUsers.All(y => y.BlockUserId != x)).ToList();

            if (filteredUsersId.Count == usersId.Count)
            {
              var blockedUserInMeetingUsers = await Context.BlockedUsers
                .Where(x => usersId.Any(y => y == x.UserId) && x.BlockUserId == user.Id && !x.IsRemoved)
                .ToListAsync();

              if (blockedUserInMeetingUsers.Count == 0)
              {
                filteredMeetings.Add(meeting);
              }
            }
          }

          return filteredMeetings.FirstOrDefault(x => IsMeetingMatchRequest(x, request, user));
        }

        foreach (var meeting in meetings)
        {
          var usersId = meeting.Users.Where(x => !x.IsRemoved).Select(x => x.UserId).ToList();
          var blockedUserInMeetingUsers = await Context.BlockedUsers
            .Where(x => usersId.Any(y => y == x.UserId) && x.BlockUserId == user.Id && !x.IsRemoved)
            .ToListAsync();

          if (blockedUserInMeetingUsers.Count == 0)
          {
            filteredMeetings.Add(meeting);
          }
        }

        return filteredMeetings.FirstOrDefault(x => IsMeetingMatchRequest(x, request, user));
      }

      return default(Meeting);
    }

    public async Task<IList<Meeting>> FindAllCloseToPreferencesWithUsersDetails(int userId, double latitude, double longitude)
    {
      var user = await Context.Users
        .Include(x => x.Profile)
        .FirstOrDefaultAsync(x => x.Id == userId && !x.IsRemoved);

      var meetings = await Context.Meetings
        .Include(x => x.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Users)
        .ThenInclude(x => x.MeetingRequest)
        .Include(x => x.DrinkType)
        .Where(x => !x.IsRemoved &&
                    x.Users.Count(y => !y.IsRemoved) < 4)
        .ToListAsync();

      if (meetings.Count > 0)
      {
        var blockedUsers = await Context.BlockedUsers
          .Where(x => x.UserId == user.Id && !x.IsRemoved)
          .ToListAsync();

        var filteredMeetings = new List<Meeting>();

        if (blockedUsers.Count > 0)
        {
          foreach (var meeting in meetings)
          {
            var usersId = meeting.Users.Where(x => !x.IsRemoved).Select(x => x.UserId).ToList();
            var filteredUsersId = usersId.Where(x => blockedUsers.All(y => y.BlockUserId != x)).ToList();

            if (filteredUsersId.Count == usersId.Count)
            {
              var blockedUserInMeetingUsers = await Context.BlockedUsers
                .Where(x => usersId.Any(y => y == x.UserId) && x.BlockUserId == user.Id && !x.IsRemoved)
                .ToListAsync();

              if (blockedUserInMeetingUsers.Count == 0)
              {
                filteredMeetings.Add(meeting);
              }
            }
          }

          var matchingNonBlockedMeetings = filteredMeetings.Where(x => IsMeetingClose(x, user, latitude, longitude)).ToList();
          matchingNonBlockedMeetings.ForEach(x => x.Users = x.Users.Where(y => !y.IsRemoved).ToList());
          foreach (var meeting in matchingNonBlockedMeetings)
          {
            foreach (var meetingUser in meeting.Users)
            {
              var userPhotos = await Context.UserProfilePhotos
                .Where(x => x.ProfileId == meetingUser.User.Profile.Id)
                .OrderBy(x => x.Order)
                .ToListAsync();

              meetingUser.User.Profile.Photos = userPhotos;
            }
          }

          return matchingNonBlockedMeetings;
        }

        foreach (var meeting in meetings)
        {
          var usersId = meeting.Users.Where(x => !x.IsRemoved).Select(x => x.UserId).ToList();
          var blockedUserInMeetingUsers = await Context.BlockedUsers
            .Where(x => usersId.Any(y => y == x.UserId) && x.BlockUserId == user.Id && !x.IsRemoved)
            .ToListAsync();

          if (blockedUserInMeetingUsers.Count == 0)
          {
            filteredMeetings.Add(meeting);
          }
        }

        var matchingMeetings = filteredMeetings.Where(x => IsMeetingClose(x, user, latitude, longitude)).ToList();
        matchingMeetings.ForEach(x => x.Users = x.Users.Where(y => !y.IsRemoved).ToList());
        foreach (var meeting in matchingMeetings)
        {
          foreach (var meetingUser in meeting.Users)
          {
            var userPhotos = await Context.UserProfilePhotos
              .Where(x => x.ProfileId == meetingUser.User.Profile.Id)
              .OrderBy(x => x.Order)
              .ToListAsync();

            meetingUser.User.Profile.Photos = userPhotos;
          }
        }

        return matchingMeetings;
      }

      return new List<Meeting>();
    }

    public async Task<Meeting> FindOneForUserWithUsersDetails(int meetingId, int userId)
    {
      return await Context.Meetings
        .Include(x => x.Users)
        .ThenInclude(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Users)
        .ThenInclude(x => x.MeetingRequest)
        .ThenInclude(x => x.DrinkTypes)
        .ThenInclude(x => x.DrinkType)
        .FirstOrDefaultAsync(x => x.Id == meetingId &&
                                  !x.Users.Any(y => y.UserId == userId && !y.IsRemoved) &&
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

    private static bool IsMeetingMatchRequest(Meeting meeting, MeetingRequest request, User requestUser)
    {
      return meeting.Users.Where(x => !x.IsRemoved).All(x => x.User.Profile.IsWithinMeetingRequestAgeRange(request)) &&
             meeting.Users.Where(x => !x.IsRemoved).All(x => requestUser.Profile.IsWithinMeetingRequestAgeRange(x.MeetingRequest)) &&
             meeting.GetDistance(request) <= 10 &&
             request.DrinkTypes.Any(x => x.DrinkTypeId == meeting.DrinkTypeId);
    }

    private static bool IsMeetingClose(Meeting meeting, User user, double latitude, double longitude)
    {
      return meeting.Users.Where(x => !x.IsRemoved).All(x => user.Profile.IsWithinMeetingRequestAgeRange(x.MeetingRequest)) &&
             meeting.GetDistance(latitude, longitude) <= 10;
    }
  }
}
