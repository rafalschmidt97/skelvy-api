using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Skelvy.Application.Uploads.Infrastructure.Uploads;
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

      if (fileData.Length > maxFileSize)
      {
        throw new BadRequestException("Max file size exceeded.");
      }

      var name = Guid.NewGuid() + Path.GetExtension(fileName);

      var account = CloudStorageAccount.Parse(_configuration["SKELVY_STORAGE_CONNECTION"]);
      var client = account.CreateCloudBlobClient();
      var container = client.GetContainerReference(_configuration["SKELVY_STORAGE_PHOTOS_CONTAINER"]);
      await container.CreateIfNotExistsAsync();
      var blob = container.GetBlockBlobReference(name);
      await blob.UploadFromStreamAsync(fileData);

      return blob.Uri.AbsoluteUri;
    }
  }
}
