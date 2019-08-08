using System.Threading.Tasks;

namespace Skelvy.Persistence.Migrations.Scripts
{
  public interface IMigration
  {
    Task Migrate();
  }
}
