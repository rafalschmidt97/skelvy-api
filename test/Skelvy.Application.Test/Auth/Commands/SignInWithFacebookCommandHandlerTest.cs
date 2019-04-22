using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Commands.SignInWithFacebook;
using Skelvy.Application.Auth.Infrastructure.Facebook;
using Skelvy.Application.Auth.Infrastructure.Repositories;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Application.Core.Persistence;
using Skelvy.Application.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;
using Xunit;

namespace Skelvy.Application.Test.Auth.Commands
{
  public class SignInWithFacebookCommandHandlerTest : DatabaseRequestTestBase
  {
    private const string AuthToken = "Token";
    private const string RefreshToken = "Token";
    private readonly AccessVerification _access;

    private readonly Mock<IFacebookService> _facebookService;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<INotificationsService> _notifications;

    public SignInWithFacebookCommandHandlerTest()
    {
      _access = new AccessVerification("facebook1", AuthToken, DateTimeOffset.UtcNow.AddDays(3), AccessTypes.Facebook);
      _facebookService = new Mock<IFacebookService>();
      _tokenService = new Mock<ITokenService>();
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldReturnTokenWithInitializedUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageTypes.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>())).ReturnsAsync(new AuthDto(AuthToken, RefreshToken));
      var handler =
        new SignInWithFacebookCommandHandler(AuthRepository(), _facebookService.Object, _tokenService.Object, _notifications.Object);

      var result = await handler.Handle(request);

      Assert.IsType<AuthDto>(result);
    }

    [Fact]
    public async Task ShouldReturnTokenWithNotInitializedUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageTypes.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      _facebookService
        .Setup(x => x.GetBody<dynamic>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync((object)GraphResponse());
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>())).ReturnsAsync(new AuthDto(AuthToken, RefreshToken));
      var handler = new SignInWithFacebookCommandHandler(AuthRepository(false), _facebookService.Object, _tokenService.Object, _notifications.Object);

      var result = await handler.Handle(request);

      Assert.IsType<AuthDto>(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotVerifiedUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageTypes.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).Throws<UnauthorizedException>();
      var handler =
        new SignInWithFacebookCommandHandler(AuthRepository(), _facebookService.Object, _tokenService.Object, _notifications.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithRemovedUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageTypes.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      var handler =
        new SignInWithFacebookCommandHandler(AuthRepositoryWithRemovedUser(), _facebookService.Object, _tokenService.Object, _notifications.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithDisabledUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageTypes.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      var handler =
        new SignInWithFacebookCommandHandler(AuthRepositoryWithDisabledUser(), _facebookService.Object, _tokenService.Object, _notifications.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    private IAuthRepository AuthRepositoryWithRemovedUser()
    {
      var context = DbContext();
      SkelvyInitializer.Initialize(context);
      var user = context.Users.FirstOrDefault(x => x.FacebookId == _access.UserId);

      if (user != null)
      {
        user.Remove(DateTimeOffset.UtcNow);
      }

      context.SaveChanges();

      return new AuthRepository(context);
    }

    private IAuthRepository AuthRepositoryWithDisabledUser()
    {
      var context = DbContext();
      SkelvyInitializer.Initialize(context);
      var user = context.Users.FirstOrDefault(x => x.FacebookId == _access.UserId);

      if (user != null)
      {
        user.Disable("Test");
      }

      context.SaveChanges();

      return new AuthRepository(context);
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
