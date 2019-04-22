using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Drinks.Infrastructure.Repositories
{
  public interface IDrinksRepository : IBaseRepository
  {
    Task<IList<Drink>> FindAll();
  }
}
