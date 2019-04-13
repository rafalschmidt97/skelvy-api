using System.IO;
using System.Threading.Tasks;

namespace Skelvy.Application.Uploads.Infrastructure.LocalUploads
{
  public interface IUploadService
  {
    Task<string> Upload(Stream fileData, string fileName, string serverPath);
  }
}
