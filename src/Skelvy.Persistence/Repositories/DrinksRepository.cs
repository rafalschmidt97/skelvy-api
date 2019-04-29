using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Drinks.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class DrinksRepository : BaseRepository, IDrinksRepository
  {
    public DrinksRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<Drink>> FindAll()
    {
      return await Context.Drinks.OrderBy(x => x.Name).ToListAsync();
    }
  }
}
