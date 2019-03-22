using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Commands.SignInWithGoogle;
using Skelvy.Application.Infrastructure.Google;
using Skelvy.Application.Infrastructure.Tokens;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Xunit;

namespace Skelvy.Application.Test.Auth.Commands
{
  public class SignInWithGoogleCommandHandlerTest : RequestTestBase
  {
    private const string AuthToken = "Token";
    private const string Token = "ABC";
    private static readonly AccessVerification Access = new AccessVerification { UserId = "1" };

    private readonly Mock<IGoogleService> _googleService;
    private readonly Mock<ITokenService> _tokenService;

    public SignInWithGoogleCommandHandlerTest()
    {
      _googleService = new Mock<IGoogleService>();
      _tokenService = new Mock<ITokenService>();
    }

    [Fact]
    public async Task ShouldReturnTokenWithInitializedUser()
    {
      var request = new SignInWithGoogleCommand { AuthToken = AuthToken };
      _googleService.Setup(x => x.Verify(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(Access);
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>(), It.IsAny<AccessVerification>())).Returns(Token);
      var handler =
        new SignInWithGoogleCommandHandler(InitializedDbContext(), _googleService.Object, _tokenService.Object);

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.IsType<string>(result);
    }

    [Fact]
    public async Task ShouldReturnTokenWithNotInitializedUser()
    {
      var request = new SignInWithGoogleCommand { AuthToken = AuthToken };
      _googleService.Setup(x => x.Verify(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(Access);
      _googleService
        .Setup(x => x.GetBody<dynamic>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
        .ReturnsAsync((object)PeopleResponse());
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>(), It.IsAny<AccessVerification>())).Returns(Token);
      var handler = new SignInWithGoogleCommandHandler(DbContext(), _googleService.Object, _tokenService.Object);

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.IsType<string>(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new SignInWithGoogleCommand { AuthToken = AuthToken };
      _googleService.Setup(x => x.Verify(It.IsAny<string>(), CancellationToken.None)).Throws<UnauthorizedException>();
      var handler =
        new SignInWithGoogleCommandHandler(InitializedDbContext(), _googleService.Object, _tokenService.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    private static dynamic PeopleResponse()
    {
      dynamic graphResponse = new ExpandoObject();
      graphResponse.emails = new ExpandoObject[1];
      graphResponse.emails[0] = new ExpandoObject();
      graphResponse.emails[0].value = "example@gmail.com";
      graphResponse.name = new ExpandoObject();
      graphResponse.name.givenName = "Example";
      graphResponse.birthday = "1997-04-22";
      graphResponse.gender = "male";
      graphResponse.image = new ExpandoObject();
      graphResponse.image.url = "Url";
      return graphResponse;
    }
  }
}
