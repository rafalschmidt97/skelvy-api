using System.Threading;
using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Infrastructure.Tokens
{
  public interface ITokenService
  {
    Task<Token> Generate(User user, CancellationToken cancellationToken);
    Task<Token> Generate(string refreshToken, CancellationToken cancellationToken);
    Task Invalidate(string refreshToken, CancellationToken cancellationToken);
  }
}
