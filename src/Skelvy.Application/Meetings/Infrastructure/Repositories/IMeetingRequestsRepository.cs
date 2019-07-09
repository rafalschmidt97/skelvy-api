using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingRequestsRepository : IBaseRepository
  {
    Task<MeetingRequest> FindOneSearchingByUserId(int userId);
    Task<MeetingRequest> FindOneWithDrinkTypesByUserId(int userId);
    Task<IList<MeetingRequest>> FindAllWithRemovedByUsersId(IEnumerable<int> usersId);
    Task<IList<MeetingRequest>> FindAllSearchingAfterOrEqualMaxDate(DateTimeOffset maxDate);
    Task<IList<MeetingRequest>> FindAllSearchingWithUsersDetailsAndDrinkTypes();
    Task<bool> ExistsOne(int requestId);
    Task<bool> ExistsOneByUserId(int userId);
    Task<bool> ExistsOneFoundByUserId(int userId);
    Task<MeetingRequest> FindOneMatchingUserRequest(User user, MeetingRequest request);
    Task<IList<MeetingRequest>> FindAllCloseToPreferencesWithUserDetails(int userId, double latitude, double longitude);
    Task<MeetingRequest> FindOneForUserWithUserDetails(int meetingRequestId, int userId);
    Task Add(MeetingRequest request);
    Task Update(MeetingRequest request);
    Task UpdateRange(IList<MeetingRequest> requests);
    Task RemoveRange(IList<MeetingRequest> requests);
  }
}
