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
    Task<MeetingRequest> FindOneWithDrinksByUserId(int userId);
    Task<IList<MeetingRequest>> FindAllWithRemovedByUsersId(IEnumerable<int> usersId);
    Task<IList<MeetingRequest>> FindAllSearchingAfterMaxDate(DateTimeOffset maxDate);
    Task<IList<MeetingRequest>> FindAllSearchingWithUsersDetailsAndDrinks();
    Task<bool> ExistsOneByUserId(int userId);
    Task<MeetingRequest> FindOneMatchingUserRequest(User user, MeetingRequest request);
    Task Add(MeetingRequest request);
    Task Update(MeetingRequest request);
    Task UpdateRange(IList<MeetingRequest> requests);
    Task RemoveRange(IList<MeetingRequest> requests);
  }
}
