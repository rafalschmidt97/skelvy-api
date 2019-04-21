using System.Threading.Tasks;
using Skelvy.Application.Auth.Commands;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Auth.Infrastructure.Tokens
{
  public interface ITokenService
  {
    Task<AuthDto> Generate(User user);
    Task<AuthDto> Generate(string refreshToken);
    Task Invalidate(string refreshToken);
  }
}
