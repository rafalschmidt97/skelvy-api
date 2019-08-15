using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingsRepository : IBaseRepository
  {
    Task<bool> ExistsOne(int id);
    Task<Meeting> FindOne(int id);
    Task<IList<Meeting>> FindAllWithActivityByUserId(int userId);
    Task<Meeting> FindOneByGroupId(int groupId);
    Task<Meeting> FindOneWithGroupByGroupId(int groupId);
    Task<IList<Meeting>> FindAllAfterOrEqualDate(DateTimeOffset maxDate);
    Task<IList<Meeting>> FindAllCloseToPreferencesWithUsersDetailsByUserIdAndLocation(int userId, double latitude, double longitude);
    Task<Meeting> FindOneNotBelongingByMeetingIdAndUserId(int meetingId, int userId);
    Task Add(Meeting meeting);
    Task Update(Meeting meeting);
    Task UpdateRange(IList<Meeting> meetings);
  }
}
