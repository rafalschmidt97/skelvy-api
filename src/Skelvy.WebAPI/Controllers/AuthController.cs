using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Auth.Commands.Logout;
using Skelvy.Application.Auth.Commands.RefreshToken;
using Skelvy.Application.Auth.Commands.SignInWithFacebook;
using Skelvy.Application.Auth.Commands.SignInWithGoogle;
using Skelvy.Application.Infrastructure.Tokens;

namespace Skelvy.WebAPI.Controllers
{
  public class AuthController : BaseController
  {
    [HttpPost("facebook")]
    [AllowAnonymous]
    public async Task<Token> SignInWithFacebook(SignInWithFacebookCommand request)
    {
      return await Mediator.Send(request, HttpContext.RequestAborted);
    }

    [HttpPost("google")]
    [AllowAnonymous]
    public async Task<Token> SignInWithGoogle(SignInWithGoogleCommand request)
    {
      return await Mediator.Send(request, HttpContext.RequestAborted);
    }

    [HttpPost("logout")]
    public async Task Logout(LogoutCommand request)
    {
      await Mediator.Send(request, HttpContext.RequestAborted);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<Token> Refresh(RefreshTokenCommand request)
    {
      return await Mediator.Send(request, HttpContext.RequestAborted);
    }
  }
}
