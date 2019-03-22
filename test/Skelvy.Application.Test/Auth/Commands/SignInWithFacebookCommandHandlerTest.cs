using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Commands.SignInWithFacebook;
using Skelvy.Application.Infrastructure.Facebook;
using Skelvy.Application.Infrastructure.Tokens;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Xunit;

namespace Skelvy.Application.Test.Auth.Commands
{
  public class SignInWithFacebookCommandHandlerTest : RequestTestBase
  {
    private const string AuthToken = "Token";
    private const string Token = "ABC";
    private static readonly AccessVerification Access = new AccessVerification { UserId = "1" };

    private readonly Mock<IFacebookService> _facebookService;
    private readonly Mock<ITokenService> _tokenService;

    public SignInWithFacebookCommandHandlerTest()
    {
      _facebookService = new Mock<IFacebookService>();
      _tokenService = new Mock<ITokenService>();
    }

    [Fact]
    public async Task ShouldReturnTokenWithInitializedUser()
    {
      var request = new SignInWithFacebookCommand { AuthToken = AuthToken };
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(Access);
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>(), It.IsAny<AccessVerification>())).Returns(Token);
      var handler =
        new SignInWithFacebookCommandHandler(InitializedDbContext(), _facebookService.Object, _tokenService.Object);

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.IsType<string>(result);
    }

    [Fact]
    public async Task ShouldReturnTokenWithNotInitializedUser()
    {
      var request = new SignInWithFacebookCommand { AuthToken = AuthToken };
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(Access);
      _facebookService
        .Setup(x => x.GetBody<dynamic>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync((object)GraphResponse());
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>(), It.IsAny<AccessVerification>())).Returns(Token);
      var handler = new SignInWithFacebookCommandHandler(DbContext(), _facebookService.Object, _tokenService.Object);

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.IsType<string>(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new SignInWithFacebookCommand { AuthToken = AuthToken };
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).Throws<UnauthorizedException>();
      var handler =
        new SignInWithFacebookCommandHandler(InitializedDbContext(), _facebookService.Object, _tokenService.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    private static dynamic GraphResponse()
    {
      dynamic graphResponse = new ExpandoObject();
      graphResponse.email = "example@gmail.com";
      graphResponse.first_name = "Example";
      graphResponse.birthday = "04/22/1997";
      graphResponse.gender = "male";
      graphResponse.picture = new ExpandoObject();
      graphResponse.picture.data = new ExpandoObject();
      graphResponse.picture.data.url = "Url";
      return graphResponse;
    }
  }
}
