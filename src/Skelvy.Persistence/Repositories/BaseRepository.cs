using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Skelvy.Application.Core.Persistence;

namespace Skelvy.Persistence.Repositories
{
  public abstract class BaseRepository : IBaseRepository
  {
    protected BaseRepository(SkelvyContext context) => Context = context;
    protected SkelvyContext Context { get; }

    public IDbContextTransaction BeginTransaction()
    {
      return Context.Database.BeginTransaction();
    }

    protected async Task SaveChanges()
    {
      await Context.SaveChangesAsync();
    }
  }
}
