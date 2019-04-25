namespace Skelvy.Application.Core.Persistence
{
  public interface IBaseRepository
  {
    ISkelvyContext Context { get; }
  }
}
