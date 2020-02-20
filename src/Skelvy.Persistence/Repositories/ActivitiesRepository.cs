using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Activities.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Persistence.Repositories
{
  public class ActivitiesRepository : BaseRepository, IActivitiesRepository
  {
    public ActivitiesRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<bool> ExistsOne(int id)
    {
      return await Context.Activities.AnyAsync(x => x.Id == id);
    }

    public async Task<Activity> FindOne(int id)
    {
      return await Context.Activities.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Activity>> FindAll()
    {
      return await Context.Activities.OrderBy(x => x.Id).ToListAsync();
    }

    public async Task<IList<Activity>> FindAllWithoutRestricted()
    {
      return await Context.Activities
        .Where(x => x.Type != ActivityType.Restricted)
        .OrderBy(x => x.Id)
        .ToListAsync();
    }
  }
}
