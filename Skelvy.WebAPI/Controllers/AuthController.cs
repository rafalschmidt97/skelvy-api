using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Auth.Commands.SignInWithFacebook;

namespace Skelvy.WebAPI.Controllers
{
  [AllowAnonymous]
  public class AuthController : BaseController
  {
    [HttpPost("facebook")]
    public async Task<ActionResult> Add(SignInWithFacebookCommand request)
    {
      var token = await Mediator.Send(request);
      return Ok(new { token });
    }
  }
}
