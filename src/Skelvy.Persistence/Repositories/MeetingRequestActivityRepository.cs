using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class MeetingRequestActivityRepository : BaseRepository, IMeetingRequestActivityRepository
  {
    public MeetingRequestActivityRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<MeetingRequestActivity>> FindAllByRequestId(int requestId)
    {
      return await Context.MeetingRequestActivities
        .Where(x => x.MeetingRequestId == requestId)
        .ToListAsync();
    }

    public async Task<IList<MeetingRequestActivity>> FindAllByRequestsId(IEnumerable<int> requestsId)
    {
      return await Context.MeetingRequestActivities
        .Where(x => requestsId.Any(y => y == x.MeetingRequestId))
        .ToListAsync();
    }

    public async Task AddRange(IEnumerable<MeetingRequestActivity> activities)
    {
      await Context.MeetingRequestActivities.AddRangeAsync(activities);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<MeetingRequestActivity> activities)
    {
      Context.MeetingRequestActivities.RemoveRange(activities);
      await SaveChanges();
    }
  }
}
