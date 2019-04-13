using System.Dynamic;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Commands.SignInWithGoogle;
using Skelvy.Application.Auth.Infrastructure.Google;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Commands;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Xunit;

namespace Skelvy.Application.Test.Auth.Commands
{
  public class SignInWithGoogleCommandHandlerTest : RequestTestBase
  {
    private const string AuthToken = "Token";
    private static readonly AccessVerification Access = new AccessVerification { UserId = "1" };

    private readonly Mock<IGoogleService> _googleService;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<INotificationsService> _notifications;

    public SignInWithGoogleCommandHandlerTest()
    {
      _googleService = new Mock<IGoogleService>();
      _tokenService = new Mock<ITokenService>();
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldReturnTokenWithInitializedUser()
    {
      var request = new SignInWithGoogleCommand { AuthToken = AuthToken, Language = LanguageTypes.EN };
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(Access);
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>())).ReturnsAsync(new Token());
      var handler =
        new SignInWithGoogleCommandHandler(InitializedDbContext(), _googleService.Object, _tokenService.Object, _notifications.Object);

      var result = await handler.Handle(request);

      Assert.IsType<Token>(result);
    }

    [Fact]
    public async Task ShouldReturnTokenWithNotInitializedUser()
    {
      var request = new SignInWithGoogleCommand { AuthToken = AuthToken, Language = LanguageTypes.EN };
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(Access);
      _googleService
        .Setup(x => x.GetBody<dynamic>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync((object)PeopleResponse());
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>())).ReturnsAsync(new Token());
      var handler = new SignInWithGoogleCommandHandler(DbContext(), _googleService.Object, _tokenService.Object, _notifications.Object);

      var result = await handler.Handle(request);

      Assert.IsType<Token>(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new SignInWithGoogleCommand { AuthToken = AuthToken, Language = LanguageTypes.EN };
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).Throws<UnauthorizedException>();
      var handler =
        new SignInWithGoogleCommandHandler(InitializedDbContext(), _googleService.Object, _tokenService.Object, _notifications.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
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
