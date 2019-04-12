using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skelvy.Application.Infrastructure.Maps
{
  public interface IMapsService
  {
    Task<IList<Location>> Search(string search, string language);
    Task<IList<Location>> Search(double latitude, double longitude, string language);
  }
}
