using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;

namespace Skelvy.Persistence.Repositories
{
  public abstract class BaseRepository : IBaseRepository
  {
    protected BaseRepository(SkelvyContext context) => Context = context;
    protected SkelvyContext Context { get; }

    public async Task Commit()
    {
      await Context.SaveChangesAsync();
    }
  }
}
