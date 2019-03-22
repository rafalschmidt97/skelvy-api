using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Auth.Commands.SignInWithFacebook;
using Skelvy.Application.Auth.Commands.SignInWithGoogle;

namespace Skelvy.WebAPI.Controllers
{
  [AllowAnonymous]
  public class AuthController : BaseController
  {
    [HttpPost("facebook")]
    public async Task<IActionResult> SignInWithFacebook(SignInWithFacebookCommand request)
    {
      var token = await Mediator.Send(request, HttpContext.RequestAborted);
      return Ok(new { token });
    }

    [HttpPost("google")]
    public async Task<IActionResult> SignInWithGoogle(SignInWithGoogleCommand request)
    {
      var token = await Mediator.Send(request, HttpContext.RequestAborted);
      return Ok(new { token });
    }
  }
}
