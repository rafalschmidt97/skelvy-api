using Destructurama.Attributed;
using MediatR;

namespace Skelvy.Application.Auth.Commands.Logout
{
  public class LogoutCommand : IRequest
  {
    [LogMasked]
    public string RefreshToken { get; set; }
  }
}
