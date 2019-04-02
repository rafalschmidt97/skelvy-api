using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Skelvy.Application.Infrastructure.Maps
{
  public interface IMapsService
  {
    Task<IList<Location>> Search(string search, string language, CancellationToken cancellationToken);
    Task<IList<Location>> Search(double latitude, double longitude, string language, CancellationToken cancellationToken);
  }
}
