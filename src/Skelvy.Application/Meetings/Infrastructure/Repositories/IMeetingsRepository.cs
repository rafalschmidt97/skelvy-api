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
    Task<bool> ExistsOneByGroupId(int groupId);
    Task<int> CountOwnMeetingsByUserId(int userId);
    Task<Meeting> FindOne(int id);
    Task<IList<Meeting>> FindAllWithActivityByUserId(int userId);
    Task<Meeting> FindOneByGroupId(int groupId);
    Task<Meeting> FindOneWithGroupByGroupId(int groupId);
    Task<Meeting> FindOneWithGroupUsersByMeetingId(int meetingId);
    Task<IList<Meeting>> FindAllAfterOrEqualDate(DateTimeOffset maxDate);
    Task<IList<Meeting>> FindAllNonHiddenCloseWithUsersDetailsByUserIdAndLocationFilterBlocked(int userId, double latitude, double longitude);
    Task<Meeting> FindOneNonHiddenAndNonFullByMeetingIdAndUserId(int meetingId, int userId);
    Task<Meeting> FindOneNonFullByMeetingId(int meetingId);
    Task Add(Meeting meeting);
    Task Update(Meeting meeting);
    Task UpdateRange(IList<Meeting> meetings);
  }
}
