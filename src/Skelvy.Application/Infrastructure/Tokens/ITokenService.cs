using Skelvy.Domain.Entities;

namespace Skelvy.Application.Infrastructure.Tokens
{
  public interface ITokenService
  {
    string Generate(User user);
  }
}
