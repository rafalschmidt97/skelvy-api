using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Auth.Infrastructure.Tokens
{
  public interface ITokenService
  {
    Task<Token> Generate(User user);
    Task<Token> Generate(string refreshToken);
    Task Invalidate(string refreshToken);
  }
}
