using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Auth.Commands.Logout;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Xunit;

namespace Skelvy.Application.Test.Auth.Commands
{
  public class LogoutCommandHandlerTest : RequestTestBase
  {
    private const string RefreshToken = "Token";
    private readonly Mock<ITokenService> _tokenService;

    public LogoutCommandHandlerTest()
    {
      _tokenService = new Mock<ITokenService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new LogoutCommand { RefreshToken = RefreshToken };
      var handler = new LogoutCommandHandler(_tokenService.Object);

      await handler.Handle(request);
    }
  }
}
