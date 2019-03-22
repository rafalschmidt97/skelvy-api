using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Infrastructure.Uploads;
using Skelvy.Common.Exceptions;

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
    public async Task<IActionResult> Upload()
    {
      var files = Request.Form.Files;
      _logger.LogInformation("Request: Upload {@File}", files);

      if (files.Count > 0)
      {
        var file = files[0];
        try
        {
          var url = await _uploadService.Upload(file.OpenReadStream(), file.FileName, Request.Host.Value, HttpContext.RequestAborted);
          return Ok(new { url });
        }
        catch (CustomException exception)
        {
          _logger.LogError("Request {Status}: {Message}", exception.Status, exception.Message);
          throw;
        }
      }

      var badRequestException = new BadRequestException("Unsupported media type. File required.");
      _logger.LogError("Request {Status}: {Message}", badRequestException.Status, badRequestException.Message);
      throw badRequestException;
    }
  }
}
