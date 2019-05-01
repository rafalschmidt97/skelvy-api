using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Skelvy.Application.Uploads.Infrastructure.LocalUploads;
using Skelvy.Common.Exceptions;

namespace Skelvy.Infrastructure.Uploads
{
  public class UploadService : IUploadService
  {
    private readonly IConfiguration _configuration;

    public UploadService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public async Task<string> Upload(Stream fileData, string fileName)
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

      var name = Guid.NewGuid() + Path.GetExtension(fileName);

      var account = CloudStorageAccount.Parse(_configuration.GetConnectionString("Storage"));
      var client = account.CreateCloudBlobClient();
      var container = client.GetContainerReference(_configuration["Storage:PhotosContainer"]);
      await container.CreateIfNotExistsAsync();
      var blob = container.GetBlockBlobReference(name);
      await blob.UploadFromStreamAsync(fileData);

      return blob.Uri.AbsoluteUri;
    }
  }
}
