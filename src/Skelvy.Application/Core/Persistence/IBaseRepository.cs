using Skelvy.Persistence;

namespace Skelvy.Application.Core.Persistence
{
  public interface IBaseRepository
  {
    SkelvyContext Context { get; }
  }
}
