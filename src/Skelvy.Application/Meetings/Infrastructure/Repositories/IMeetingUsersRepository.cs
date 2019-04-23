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
    Task<IList<MeetingUser>> FindAllWithMeetingRequestByMeetingId(int meetingId);
    Task<IList<MeetingUser>> FindAllByUsersId(IEnumerable<int> usersId);
  }
}
