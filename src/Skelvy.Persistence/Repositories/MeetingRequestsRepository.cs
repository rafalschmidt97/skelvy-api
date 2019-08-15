using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Extensions;

namespace Skelvy.Persistence.Repositories
{
  public class MeetingRequestsRepository : BaseRepository, IMeetingRequestsRepository
  {
    public MeetingRequestsRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<MeetingRequest> FindOneWithExpiredById(int id)
    {
      return await Context.MeetingRequests.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<MeetingRequest> FindOneSearchingByRequestId(int requestId)
    {
      return await Context.MeetingRequests
        .FirstOrDefaultAsync(x => x.Id == requestId &&
                                  !x.IsRemoved &&
                                  x.Status == MeetingRequestStatusType.Searching);
    }

    public async Task<int> CountSearchingByUserId(int userId)
    {
      return await Context.MeetingRequests
        .CountAsync(x => x.UserId == userId && !x.IsRemoved && x.Status == MeetingRequestStatusType.Searching);
    }

    public async Task<IList<MeetingRequest>> FindAllSearchingWithActivitiesByUserId(int userId)
    {
      return await Context.MeetingRequests
        .Include(x => x.Activities)
        .ThenInclude(x => x.Activity)
        .Where(x => x.UserId == userId && !x.IsRemoved && x.Status == MeetingRequestStatusType.Searching)
        .ToListAsync();
    }

    public async Task<IList<MeetingRequest>> FindAllWithRemovedByUsersId(IEnumerable<int> usersId)
    {
      return await Context.MeetingRequests
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }

    public async Task<IList<MeetingRequest>> FindAllSearchingAfterOrEqualMaxDate(DateTimeOffset maxDate)
    {
      return await Context.MeetingRequests
        .Where(x => !x.IsRemoved && x.MaxDate <= maxDate && x.Status == MeetingRequestStatusType.Searching)
        .ToListAsync();
    }

    public async Task<IList<MeetingRequest>> FindAllSearchingWithUsersDetailsAndActivities()
    {
      return await Context.MeetingRequests
        .Include(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Activities)
        .ThenInclude(x => x.Activity)
        .Where(x => !x.IsRemoved && x.Status == MeetingRequestStatusType.Searching)
        .ToListAsync();
    }

    public async Task<bool> ExistsOne(int requestId)
    {
      return await Context.MeetingRequests
        .AnyAsync(x => x.Id == requestId && !x.IsRemoved);
    }

    public async Task<bool> ExistsOneFoundByRequestId(int requestId)
    {
      return await Context.MeetingRequests
        .AnyAsync(x => x.Id == requestId && !x.IsRemoved && x.Status == MeetingRequestStatusType.Found);
    }

    public async Task<MeetingRequest> FindOneMatchingUserRequest(User user, MeetingRequest request)
    {
      var requests = await Context.MeetingRequests
        .Include(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Activities)
        .ThenInclude(x => x.Activity)
        .Where(x => !x.IsRemoved &&
                    x.Id != request.Id &&
                    x.Status == MeetingRequestStatusType.Searching &&
                    x.MinDate <= request.MaxDate &&
                    x.MaxDate >= request.MinDate)
        .ToListAsync();

      return requests.FirstOrDefault(x => AreRequestsMatch(x, request, user));
    }

    public async Task<IList<MeetingRequest>> FindAllCloseToPreferencesWithUserDetails(int userId, double latitude, double longitude)
    {
      var user = await Context.Users
        .Include(x => x.Profile)
        .FirstOrDefaultAsync(x => x.Id == userId && !x.IsRemoved);

      var requests = await Context.MeetingRequests
        .Include(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Activities)
        .ThenInclude(x => x.Activity)
        .Where(x => x.UserId != userId &&
                    !x.IsRemoved &&
                    x.Status == MeetingRequestStatusType.Searching)
        .ToListAsync();

      if (requests.Any())
      {
        foreach (var request in requests)
        {
          var userPhotos = await Context.ProfilePhotos
            .Include(x => x.Attachment)
            .Where(x => x.ProfileId == request.User.Profile.Id)
            .OrderBy(x => x.Order)
            .ToListAsync();

          request.User.Profile.Photos = userPhotos;
        }

        return requests.Where(x => AreMeetingRequestClose(x, user, latitude, longitude)).ToList();
      }

      return new List<MeetingRequest>();
    }

    public async Task<MeetingRequest> FindOneSearchingWithUserDetailsByRequestId(int requestId)
    {
      return await Context.MeetingRequests
        .Include(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Activities)
        .ThenInclude(x => x.Activity)
        .FirstOrDefaultAsync(x => x.Id == requestId &&
                                  !x.IsRemoved &&
                                  x.Status == MeetingRequestStatusType.Searching);
    }

    public async Task Add(MeetingRequest request)
    {
      await Context.MeetingRequests.AddAsync(request);
      await SaveChanges();
    }

    public async Task Update(MeetingRequest request)
    {
      Context.MeetingRequests.Update(request);
      await SaveChanges();
    }

    public async Task UpdateRange(IList<MeetingRequest> requests)
    {
      Context.MeetingRequests.UpdateRange(requests);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<MeetingRequest> requests)
    {
      Context.MeetingRequests.RemoveRange(requests);
      await SaveChanges();
    }

    private static bool AreRequestsMatch(MeetingRequest request1, MeetingRequest request2, User requestUser)
    {
      return request1.User.Profile.IsWithinMeetingRequestAgeRange(request2) &&
             requestUser.Profile.IsWithinMeetingRequestAgeRange(request1) &&
             request2.Activities.Any(x => request1.Activities.Any(y => y.ActivityId == x.ActivityId) && request1.GetDistance(request2) <= x.Activity.Distance);
    }

    private static bool AreMeetingRequestClose(MeetingRequest request, User user, double latitude, double longitude)
    {
      return user.Profile.IsWithinMeetingRequestAgeRange(request) &&
             request.GetDistance(latitude, longitude) <= 15;
    }
  }
}
