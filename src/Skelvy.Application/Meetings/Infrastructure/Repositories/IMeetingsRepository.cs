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
    Task<Meeting> FindOne(int id);
    Task<IList<Meeting>> FindAllWithActivityByUserId(int userId);
    Task<Meeting> FindOneByGroupId(int groupId);
    Task<Meeting> FindOneWithGroupByGroupId(int groupId);
    Task<IList<Meeting>> FindAllAfterOrEqualDate(DateTimeOffset maxDate);
    Task<IList<Meeting>> FindAllNonHiddenCloseWithUsersDetailsByUserIdAndLocation(int userId, double latitude, double longitude);
    Task<Meeting> FindOneNonHiddenNotBelongingByMeetingIdAndUserId(int meetingId, int userId);
    Task Add(Meeting meeting);
    Task Update(Meeting meeting);
    Task UpdateRange(IList<Meeting> meetings);
  }
}
