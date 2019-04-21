using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Commands.Logout;
using Skelvy.Application.Auth.Commands.RefreshToken;
using Skelvy.Application.Auth.Commands.SignInWithFacebook;
using Skelvy.Application.Auth.Commands.SignInWithGoogle;

namespace Skelvy.WebAPI.Controllers
{
  public class AuthController : BaseController
  {
    [HttpPost("facebook")]
    [AllowAnonymous]
    public async Task<AuthDto> SignInWithFacebook(SignInWithFacebookCommand request)
    {
      return await Mediator.Send(request);
    }

    [HttpPost("google")]
    [AllowAnonymous]
    public async Task<AuthDto> SignInWithGoogle(SignInWithGoogleCommand request)
    {
      return await Mediator.Send(request);
    }

    [HttpPost("logout")]
    public async Task Logout(LogoutCommand request)
    {
      await Mediator.Send(request);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<AuthDto> Refresh(RefreshTokenCommand request)
    {
      return await Mediator.Send(request);
    }
  }
}
