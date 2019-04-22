using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Core.Persistence
{
  public class MeetingRequestsRepository : BaseRepository, IMeetingRequestsRepository
  {
    public MeetingRequestsRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<MeetingRequest> FindOneWithDrinksByUserId(int userId)
    {
      return await Context.MeetingRequests
        .Include(x => x.Drinks)
        .ThenInclude(x => x.Drink)
        .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsRemoved);
    }
  }
}
