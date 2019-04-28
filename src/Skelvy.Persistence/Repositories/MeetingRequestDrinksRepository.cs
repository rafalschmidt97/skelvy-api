using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class MeetingRequestDrinksRepository : BaseRepository, IMeetingRequestDrinksRepository
  {
    public MeetingRequestDrinksRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<MeetingRequestDrink>> FindAllByRequestsId(IEnumerable<int> requestsId)
    {
      return await Context.MeetingRequestDrinks
        .Where(x => requestsId.Any(y => y == x.MeetingRequestId))
        .ToListAsync();
    }

    public void RemoveRangeAsTransaction(IList<MeetingRequestDrink> drinks)
    {
      Context.MeetingRequestDrinks.RemoveRange(drinks);
    }

    public void AddRangeAsTransaction(IEnumerable<MeetingRequestDrink> drinks)
    {
      Context.MeetingRequestDrinks.AddRange(drinks);
    }
  }
}
