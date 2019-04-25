using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Persistence;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class MeetingUsersRepository : BaseRepository, IMeetingUsersRepository
  {
    public MeetingUsersRepository(ISkelvyContext context)
      : base(context)
    {
    }

    public async Task<MeetingUser> FindOneByUserId(int userId)
    {
      return await Context.MeetingUsers
        .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRemoved);
    }

    public async Task<MeetingUser> FindOneWithMeetingByUserId(int userId)
    {
      return await Context.MeetingUsers
        .Include(x => x.Meeting)
        .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRemoved);
    }

    public async Task<IList<MeetingUser>> FindAllByMeetingId(int meetingId)
    {
      return await Context.MeetingUsers
        .Where(x => x.MeetingId == meetingId && !x.IsRemoved)
        .ToListAsync();
    }

    public async Task<IList<MeetingUser>> FindAllWithMeetingRequestByMeetingId(int meetingId)
    {
      return await Context.MeetingUsers
        .Include(x => x.MeetingRequest)
        .Where(x => x.MeetingId == meetingId && !x.IsRemoved)
        .ToListAsync();
    }

    public async Task<IList<MeetingUser>> FindAllWithMeetingRequestByMeetingsId(IEnumerable<int> meetingsId)
    {
      return await Context.MeetingUsers
        .Include(x => x.MeetingRequest)
        .Where(x => meetingsId.Any(y => y == x.MeetingId) && !x.IsRemoved)
        .ToListAsync();
    }

    public async Task<IList<MeetingUser>> FindAllWithRemovedByUsersId(IEnumerable<int> usersId)
    {
      return await Context.MeetingUsers
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }

    public async Task<bool> ExistsOneByUserId(int userId)
    {
      return await Context.MeetingUsers
        .AnyAsync(x => x.UserId == userId && !x.IsRemoved);
    }
  }
}
