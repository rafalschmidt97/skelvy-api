using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Commands.SignInWithGoogle;
using Skelvy.Application.Auth.Infrastructure.Google;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Initializers;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;
using Skelvy.Persistence;
using Xunit;

namespace Skelvy.Application.Test.Auth.Commands
{
  public class SignInWithGoogleCommandHandlerTest : RequestTestBase
  {
    private const string AuthToken = "Token";
    private const string RefreshToken = "Token";
    private readonly AccessVerification _access;

    private readonly Mock<IGoogleService> _googleService;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<INotificationsService> _notifications;

    public SignInWithGoogleCommandHandlerTest()
    {
      _access = new AccessVerification("google1", AuthToken, DateTimeOffset.UtcNow.AddDays(3), AccessTypes.Google);
      _googleService = new Mock<IGoogleService>();
      _tokenService = new Mock<ITokenService>();
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldReturnTokenWithInitializedUser()
    {
      var request = new SignInWithGoogleCommand(AuthToken, LanguageTypes.EN);
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>())).ReturnsAsync(new AuthDto(AuthToken, RefreshToken));
      var handler =
        new SignInWithGoogleCommandHandler(InitializedDbContext(), _googleService.Object, _tokenService.Object, _notifications.Object);

      var result = await handler.Handle(request);

      Assert.IsType<AuthDto>(result);
    }

    [Fact]
    public async Task ShouldReturnTokenWithNotInitializedUser()
    {
      var request = new SignInWithGoogleCommand(AuthToken, LanguageTypes.EN);
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      _googleService
        .Setup(x => x.GetBody<dynamic>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync((object)PeopleResponse());
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>())).ReturnsAsync(new AuthDto(AuthToken, RefreshToken));
      var handler = new SignInWithGoogleCommandHandler(DbContext(), _googleService.Object, _tokenService.Object, _notifications.Object);

      var result = await handler.Handle(request);

      Assert.IsType<AuthDto>(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotVerifiedUser()
    {
      var request = new SignInWithGoogleCommand(AuthToken, LanguageTypes.EN);
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).Throws<UnauthorizedException>();
      var handler =
        new SignInWithGoogleCommandHandler(InitializedDbContext(), _googleService.Object, _tokenService.Object, _notifications.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithRemovedUser()
    {
      var request = new SignInWithGoogleCommand(AuthToken, LanguageTypes.EN);
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).Throws<UnauthorizedException>();
      var handler =
        new SignInWithGoogleCommandHandler(InitializedDbContextWithRemovedUser(), _googleService.Object, _tokenService.Object, _notifications.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithDisabledUser()
    {
      var request = new SignInWithGoogleCommand(AuthToken, LanguageTypes.EN);
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).Throws<UnauthorizedException>();
      var handler =
        new SignInWithGoogleCommandHandler(InitializedDbContextWithDisabledUser(), _googleService.Object, _tokenService.Object, _notifications.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    private SkelvyContext InitializedDbContextWithRemovedUser()
    {
      var context = DbContext();
      SkelvyInitializer.Initialize(context);
      var user = context.Users.FirstOrDefault(x => x.FacebookId == _access.UserId);

      if (user != null)
      {
        user.Remove(DateTimeOffset.UtcNow);
      }

      context.SaveChanges();

      return context;
    }

    private SkelvyContext InitializedDbContextWithDisabledUser()
    {
      var context = DbContext();
      SkelvyInitializer.Initialize(context);
      var user = context.Users.FirstOrDefault(x => x.FacebookId == _access.UserId);

      if (user != null)
      {
        user.Disable("Test");
      }

      context.SaveChanges();

      return context;
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
