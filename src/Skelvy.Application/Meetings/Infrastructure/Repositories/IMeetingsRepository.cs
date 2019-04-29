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
    Task<IList<Meeting>> FindAllAfterDate(DateTimeOffset maxDate);
    Task<Meeting> FindOneMatchingUserRequest(User user, MeetingRequest request);
    Task Add(Meeting meeting);
    Task Update(Meeting meeting);
    Task UpdateRange(IList<Meeting> meetings);
  }
}
