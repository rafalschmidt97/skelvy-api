using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class MeetingRequestDrinkTypesRepository : BaseRepository, IMeetingRequestDrinkTypesRepository
  {
    public MeetingRequestDrinkTypesRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<MeetingRequestDrinkType>> FindAllByRequestsId(IEnumerable<int> requestsId)
    {
      return await Context.MeetingRequestDrinkTypes
        .Where(x => requestsId.Any(y => y == x.MeetingRequestId))
        .ToListAsync();
    }

    public async Task AddRange(IEnumerable<MeetingRequestDrinkType> drinkTypes)
    {
      await Context.MeetingRequestDrinkTypes.AddRangeAsync(drinkTypes);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<MeetingRequestDrinkType> drinkTypes)
    {
      Context.MeetingRequestDrinkTypes.RemoveRange(drinkTypes);
      await SaveChanges();
    }
  }
}
