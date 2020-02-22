using System.Threading.Tasks;

namespace Skelvy.Persistence.ManualMigrations
{
  public interface IMigration
  {
    Task Migrate();
  }
}
