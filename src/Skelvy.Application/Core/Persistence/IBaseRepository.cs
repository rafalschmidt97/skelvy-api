using System.Threading.Tasks;

namespace Skelvy.Application.Core.Persistence
{
  public interface IBaseRepository
  {
    Task Commit();
  }
}
