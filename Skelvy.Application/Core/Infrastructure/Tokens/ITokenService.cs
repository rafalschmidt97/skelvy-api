using Skelvy.Application.Auth.Commands;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Core.Infrastructure.Tokens
{
  public interface ITokenService
  {
    string Generate(User user, AccessVerification accessVerification);
  }
}
