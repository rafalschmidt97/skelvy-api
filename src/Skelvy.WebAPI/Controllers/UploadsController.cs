using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Uploads.Commands.UploadPhoto;
using Skelvy.Common.Exceptions;

namespace Skelvy.WebAPI.Controllers
{
  public class UploadsController : BaseController
  {
    [HttpPost]
    public async Task<IActionResult> Upload()
    {
      var files = Request.Form.Files;

      if (files.Count <= 0)
      {
        throw new BadRequestException("Unsupported media type. File required.");
      }

      var file = files[0];
      var request = new UploadPhotoCommand
      {
        Data = file.OpenReadStream(),
        Name = file.FileName,
        ServerPath = Request.Host.Value,
      };

      var url = await Mediator.Send(request);
      return Ok(new { url });
    }
  }
}
