using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Drinks.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Core.Persistence
{
  public class DrinksRepository : BaseRepository, IDrinksRepository
  {
    public DrinksRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<Drink>> FindAll()
    {
      return await Context.Drinks.ToListAsync();
    }
  }
}
