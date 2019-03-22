using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Infrastructure.Uploads;
using Skelvy.Common.Exceptions;

namespace Skelvy.Infrastructure.Uploads
{
  public class UploadService : IUploadService
  {
    public async Task<string> Upload(Stream fileData, string fileName, string serverPath, CancellationToken cancellationToken)
    {
      const int maxFileSize = 5 * 1024 * 1024; // 5MB
      var acceptedFileTypes = new[] { ".jpg", ".jpeg", ".png", ".heic" };

      if (fileData.Length > maxFileSize)
      {
        throw new BadRequestException("Max file size exceeded.");
      }

      if (acceptedFileTypes.All(s => s != Path.GetExtension(fileName).ToLower(CultureInfo.CurrentCulture)))
      {
        throw new BadRequestException("Invalid file type.");
      }

      var relativePath = Path.Combine("wwwroot", "uploads");
      var listeningPath = Path.Combine(serverPath, "uploads");

      if (!Directory.Exists(relativePath))
      {
        Directory.CreateDirectory(relativePath);
      }

      var name = Guid.NewGuid() + Path.GetExtension(fileName);
      var path = Path.Combine(relativePath, name);
      using (var stream = new FileStream(path, FileMode.Create))
      {
        await fileData.CopyToAsync(stream, cancellationToken);
      }

      return Path.Combine("http://" + listeningPath, name);
    }
  }
}
