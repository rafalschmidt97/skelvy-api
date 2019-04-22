using Skelvy.Persistence;

namespace Skelvy.Application.Core.Persistence
{
  public abstract class BaseRepository : IBaseRepository
  {
    protected BaseRepository(SkelvyContext context) => Context = context;
    public SkelvyContext Context { get; }
  }
}
