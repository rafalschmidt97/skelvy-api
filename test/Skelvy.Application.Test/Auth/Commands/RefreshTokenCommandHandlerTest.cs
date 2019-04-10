using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Auth.Commands.RefreshToken;
using Skelvy.Application.Infrastructure.Tokens;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Auth.Commands
{
  public class RefreshTokenCommandHandlerTest : RequestTestBase
  {
    private const string RefreshToken = "Token";
    private readonly Mock<ITokenService> _tokenService;

    public RefreshTokenCommandHandlerTest()
    {
      _tokenService = new Mock<ITokenService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new RefreshTokenCommand { RefreshToken = RefreshToken };
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(new Token());
      var handler = new RefreshTokenCommandHandler(_tokenService.Object);

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new RefreshTokenCommand { RefreshToken = RefreshToken };
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<string>(), CancellationToken.None)).Throws<UnauthorizedException>();
      var handler = new RefreshTokenCommandHandler(_tokenService.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
