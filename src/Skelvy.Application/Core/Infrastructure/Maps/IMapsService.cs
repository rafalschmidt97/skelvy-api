using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skelvy.Application.Core.Infrastructure.Maps
{
  public interface IMapsService
  {
    Task<ICollection<Location>> Search(string search, string language);
    Task<ICollection<Location>> Search(double latitude, double longitude, string language);
  }
}
