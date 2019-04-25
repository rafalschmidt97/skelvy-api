using System.Collections.Generic;
using System.Threading.Tasks;
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
      // return await Context.Drinks.ToListAsync();

      // return await Context.SqlQuery(
      //   @"
      //   SELECT Drink.Id, Drink.Name
      //   FROM Drinks as Drink
      //   ",
      //   data => new Drink(Convert.ToInt32(data["Id"]), Convert.ToString(data["Name"])));

      return await Context.SqlQuery<Drink>(@"
        SELECT Drink.Id, Drink.Name
        FROM Drinks as Drink
      ");
    }
  }
}
