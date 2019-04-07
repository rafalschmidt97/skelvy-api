using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Auth.Commands.Logout;
using Skelvy.Application.Auth.Commands.SignInWithFacebook;
using Skelvy.Application.Auth.Commands.SignInWithGoogle;
using Skelvy.Application.Infrastructure.Tokens;

namespace Skelvy.WebAPI.Controllers
{
  [AllowAnonymous]
  public class AuthController : BaseController
  {
    [HttpPost("facebook")]
    public async Task<Token> SignInWithFacebook(SignInWithFacebookCommand request)
    {
      return await Mediator.Send(request, HttpContext.RequestAborted);
    }

    [HttpPost("google")]
    public async Task<Token> SignInWithGoogle(SignInWithGoogleCommand request)
    {
      return await Mediator.Send(request, HttpContext.RequestAborted);
    }

    [HttpPost("logout")]
    public async Task SignInWithGoogle(LogoutCommand request)
    {
      await Mediator.Send(request, HttpContext.RequestAborted);
    }
  }
}
