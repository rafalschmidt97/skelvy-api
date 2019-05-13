using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class MeetingUsersRepository : BaseRepository, IMeetingUsersRepository
  {
    public MeetingUsersRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<MeetingUser> FindOneByUserId(int userId)
    {
      return await Context.MeetingUsers
        .Include(x => x.Meeting)
        .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRemoved && !x.Meeting.IsRemoved);
    }

    public async Task<MeetingUser> FindOneWithMeetingByUserId(int userId)
    {
      return await Context.MeetingUsers
        .Include(x => x.Meeting)
        .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRemoved && !x.Meeting.IsRemoved);
    }

    public async Task<IList<MeetingUser>> FindAllByMeetingId(int meetingId)
    {
      return await Context.MeetingUsers
        .Include(x => x.Meeting)
        .Where(x => x.MeetingId == meetingId && !x.IsRemoved && !x.Meeting.IsRemoved)
        .ToListAsync();
    }

    public async Task<IList<MeetingUser>> FindAllWithMeetingRequestByMeetingId(int meetingId)
    {
      return await Context.MeetingUsers
        .Include(x => x.Meeting)
        .Include(x => x.MeetingRequest)
        .Where(x => x.MeetingId == meetingId && !x.IsRemoved && !x.Meeting.IsRemoved)
        .ToListAsync();
    }

    public async Task<IList<MeetingUser>> FindAllWithMeetingRequestByMeetingsId(IEnumerable<int> meetingsId)
    {
      return await Context.MeetingUsers
        .Include(x => x.Meeting)
        .Include(x => x.MeetingRequest)
        .Where(x => meetingsId.Any(y => y == x.MeetingId) && !x.IsRemoved && !x.Meeting.IsRemoved)
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
      var meetingUser = await Context.MeetingUsers
        .Include(x => x.Meeting)
        .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRemoved && !x.Meeting.IsRemoved);

      return meetingUser != null;
    }

    public async Task Add(MeetingUser meetingUser)
    {
      await Context.MeetingUsers.AddAsync(meetingUser);
      await SaveChanges();
    }

    public async Task AddRange(IList<MeetingUser> meetingUsers)
    {
      await Context.MeetingUsers.AddRangeAsync(meetingUsers);
      await SaveChanges();
    }

    public async Task Update(MeetingUser meetingUser)
    {
      Context.MeetingUsers.Update(meetingUser);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<MeetingUser> meetingUsers)
    {
      Context.MeetingUsers.RemoveRange(meetingUsers);
      await SaveChanges();
    }
  }
}
