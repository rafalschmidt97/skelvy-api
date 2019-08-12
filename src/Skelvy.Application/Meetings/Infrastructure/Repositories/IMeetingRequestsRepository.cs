using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingRequestsRepository : IBaseRepository
  {
    Task<MeetingRequest> FindOneWithExpiredById(int id);
    Task<MeetingRequest> FindOneSearchingByRequestId(int requestId);
    Task<IList<MeetingRequest>> FindAllSearchingWithActivitiesByUserId(int userId);
    Task<IList<MeetingRequest>> FindAllWithRemovedByUsersId(IEnumerable<int> usersId);
    Task<IList<MeetingRequest>> FindAllSearchingAfterOrEqualMaxDate(DateTimeOffset maxDate);
    Task<IList<MeetingRequest>> FindAllSearchingWithUsersDetailsAndActivities();
    Task<bool> ExistsOne(int requestId);
    Task<bool> ExistsOneFoundByRequestId(int requestId);
    Task<MeetingRequest> FindOneMatchingUserRequest(User user, MeetingRequest request);
    Task<IList<MeetingRequest>> FindAllCloseToPreferencesWithUserDetails(int userId, double latitude, double longitude);
    Task<MeetingRequest> FindOneSearchingWithUserDetailsByRequestId(int requestId);
    Task Add(MeetingRequest request);
    Task Update(MeetingRequest request);
    Task UpdateRange(IList<MeetingRequest> requests);
    Task RemoveRange(IList<MeetingRequest> requests);
  }
}
