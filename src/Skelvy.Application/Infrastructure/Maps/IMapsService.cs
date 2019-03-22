using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Skelvy.Application.Infrastructure.Maps
{
  public interface IMapsService
  {
    Task<ICollection<Location>> Search(string search, string language, CancellationToken cancellationToken);
    Task<ICollection<Location>> Search(double latitude, double longitude, string language, CancellationToken cancellationToken);
  }
}
