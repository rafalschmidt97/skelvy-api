using System.IO;
using System.Threading.Tasks;

namespace Skelvy.Application.Core.Infrastructure.Uploads
{
  public interface IUploadService
  {
    Task<string> Upload(Stream fileData, string fileName, string serverPath);
  }
}
