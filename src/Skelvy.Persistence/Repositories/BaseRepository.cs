using Skelvy.Application.Core.Persistence;

namespace Skelvy.Persistence.Repositories
{
  public abstract class BaseRepository : IBaseRepository
  {
    protected BaseRepository(ISkelvyContext context) => Context = context;
    public ISkelvyContext Context { get; }
  }
}
