using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Persistence;
using Skelvy.Application.Drinks.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class DrinksRepository : BaseRepository, IDrinksRepository
  {
    public DrinksRepository(ISkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<Drink>> FindAll()
    {
      return await Context.Drinks.ToListAsync();
    }
  }
}
