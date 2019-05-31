using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Drinks.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class DrinkTypesRepository : BaseRepository, IDrinkTypesRepository
  {
    public DrinkTypesRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<DrinkType>> FindAll()
    {
      return await Context.DrinkTypes.OrderBy(x => x.Id).ToListAsync();
    }
  }
}
