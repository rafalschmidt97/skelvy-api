using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingsRepository : IBaseRepository
  {
    Task<Meeting> FindOneWithUsersDetailsAndDrinkByUserId(int userId);
    Task<IList<Meeting>> FindAllAfterOrEqualDate(DateTimeOffset maxDate);
    Task<Meeting> FindOneMatchingUserRequest(User user, MeetingRequest request);
    Task<IList<Meeting>> FindAllCloseToPreferences(int userId, double latitude, double longitude);
    Task Add(Meeting meeting);
    Task Update(Meeting meeting);
    Task UpdateRange(IList<Meeting> meetings);
  }
}
