using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingUsersRepository : IBaseRepository
  {
    Task<MeetingUser> FindOneByUserId(int userId);
    Task<MeetingUser> FindOneWithMeetingByUserId(int userId);
    Task<IList<MeetingUser>> FindAllByMeetingId(int meetingId);
    Task<IList<MeetingUser>> FindAllWithMeetingRequestByMeetingId(int meetingId);
    Task<IList<MeetingUser>> FindAllWithMeetingRequestByMeetingsId(IEnumerable<int> meetingsId);
    Task<IList<MeetingUser>> FindAllWithRemovedByUsersId(IEnumerable<int> usersId);
    Task<bool> ExistsOneByUserId(int userId);
    Task Add(MeetingUser meetingUser);
    Task AddRange(IList<MeetingUser> meetingUsers);
    Task Update(MeetingUser meetingUser);
    Task RemoveRange(IList<MeetingUser> meetingUsers);
  }
}
