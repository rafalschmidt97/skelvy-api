using Microsoft.EntityFrameworkCore.Storage;

namespace Skelvy.Application.Core.Persistence
{
  public interface IBaseRepository
  {
    IDbContextTransaction BeginTransaction();
  }
}
