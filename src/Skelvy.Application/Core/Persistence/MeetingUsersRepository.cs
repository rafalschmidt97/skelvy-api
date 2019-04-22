using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Core.Persistence
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
        .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRemoved);
    }
  }
}
