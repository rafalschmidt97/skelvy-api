using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Persistence;
using static Skelvy.Application.Meetings.Commands.CreateMeetingRequest.CreateMeetingRequestHelper;

namespace Skelvy.Application.Core.Persistence
{
  public class MeetingRequestsRepository : BaseRepository, IMeetingRequestsRepository
  {
    public MeetingRequestsRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<MeetingRequest> FindOneSearchingByUserId(int userId)
    {
      return await Context.MeetingRequests
        .FirstOrDefaultAsync(x => x.UserId == userId &&
                                  x.Status == MeetingRequestStatusTypes.Searching && !x.IsRemoved);
    }

    public async Task<MeetingRequest> FindOneWithDrinksByUserId(int userId)
    {
      return await Context.MeetingRequests
        .Include(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRemoved);
    }

    public async Task<IList<MeetingRequest>> FindAllWithRemovedByUsersId(IEnumerable<int> usersId)
    {
      return await Context.MeetingRequests
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }

    public async Task<IList<MeetingRequest>> FindAllSearchingAfterMaxDate(DateTimeOffset maxDate)
    {
      return await Context.MeetingRequests
        .Where(x => x.MaxDate < maxDate && x.Status == MeetingRequestStatusTypes.Searching && !x.IsRemoved)
        .ToListAsync();
    }

    public async Task<IList<MeetingRequest>> FindAllSearchingWithUsersDetailsAndDrinks()
    {
      return await Context.MeetingRequests
        .Include(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .Where(x => x.Status == MeetingRequestStatusTypes.Searching && !x.IsRemoved)
        .ToListAsync();
    }

    public async Task<bool> ExistsOneByUserId(int userId)
    {
      return await Context.MeetingRequests
        .AnyAsync(x => x.UserId == userId && !x.IsRemoved);
    }

    public async Task<MeetingRequest> FindOneMatchingUserRequest(User user, MeetingRequest request)
    {
      var requests = await Context.MeetingRequests
        .Include(x => x.User)
        .ThenInclude(x => x.Profile)
        .Include(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .Where(x => x.Id != request.Id &&
                    x.Status == MeetingRequestStatusTypes.Searching &&
                    !x.IsRemoved &&
                    x.MinDate <= request.MaxDate &&
                    x.MaxDate >= request.MinDate)
        .ToListAsync();

      return requests.FirstOrDefault(x => AreRequestsMatch(x, request, user));
    }

    private static bool AreRequestsMatch(MeetingRequest request1, MeetingRequest request2, User requestUser)
    {
      return IsUserAgeWithinMeetingRequestAgeRange(CalculateAge(request1.User.Profile.Birthday), request2.MinAge, request2.MaxAge) &&
             IsUserAgeWithinMeetingRequestAgeRange(CalculateAge(requestUser.Profile.Birthday), request1.MinAge, request1.MaxAge) &&
             CalculateDistance(
               request1.Latitude,
               request1.Longitude,
               request2.Latitude,
               request2.Longitude) <= 5 &&
             request2.Drinks.Any(x => request1.Drinks.Any(y => y.Drink.Id == x.DrinkId));
    }

    private static bool IsUserAgeWithinMeetingRequestAgeRange(int age, int minAge, int maxAge)
    {
      return age >= minAge && (maxAge >= 55 || age <= maxAge);
    }
  }
}
