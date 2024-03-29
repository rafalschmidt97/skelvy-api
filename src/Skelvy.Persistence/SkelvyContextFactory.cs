using Microsoft.EntityFrameworkCore;
using Skelvy.Persistence.Helpers;

namespace Skelvy.Persistence
{
  public class SkelvyContextFactory : DesignTimeDbContextFactoryBase<SkelvyContext>
  {
    protected override SkelvyContext CreateNewInstance(DbContextOptions<SkelvyContext> options)
    {
      return new SkelvyContext(options);
    }
  }
}
