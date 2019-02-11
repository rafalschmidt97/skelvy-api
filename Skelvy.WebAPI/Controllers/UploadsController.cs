using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Core.Infrastructure.Uploads;

namespace Skelvy.WebAPI.Controllers
{
  public class UploadsController : BaseController
  {
    private readonly IUploadService _uploadService;
    private readonly ILogger<UploadsController> _logger;

    public UploadsController(IUploadService uploadService, ILogger<UploadsController> logger)
    {
      _uploadService = uploadService;
      _logger = logger;
    }

    [HttpPost]
    public async Task<string> Upload(IFormFile file)
    {
      _logger.LogInformation("Request: Upload {@File}", file);

      try
      {
        return await _uploadService.Upload(file.OpenReadStream(), file.FileName, Request.Host.Value);
      }
      catch (CustomException exception)
      {
        _logger.LogError("Request {Status}: {Message}", exception.Status, exception.Message);
        throw;
      }
    }
  }
}
