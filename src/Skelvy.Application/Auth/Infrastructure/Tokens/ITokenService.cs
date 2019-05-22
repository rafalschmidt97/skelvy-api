using System.Threading.Tasks;
using Skelvy.Application.Auth.Commands;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Auth.Infrastructure.Tokens
{
  public interface ITokenService
  {
    Task<TokenDto> Generate(User user);
    Task<TokenDto> Generate(string refreshToken);
    Task Invalidate(string refreshToken);
  }
}
