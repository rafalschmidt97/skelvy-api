using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Commands.SignInWithFacebook;
using Skelvy.Application.Auth.Infrastructure.Facebook;
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
  public class SignInWithFacebookCommandHandlerTest : RequestTestBase
  {
    private const string AuthToken = "Token";
    private const string RefreshToken = "Token";
    private static readonly AccessVerification Access =
      new AccessVerification("1", AuthToken, DateTimeOffset.UtcNow.AddDays(3), AccessTypes.Facebook);

    private readonly Mock<IFacebookService> _facebookService;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<INotificationsService> _notifications;

    public SignInWithFacebookCommandHandlerTest()
    {
      _facebookService = new Mock<IFacebookService>();
      _tokenService = new Mock<ITokenService>();
      _notifications = new Mock<INotificationsService>();
    }

    [Fact]
    public async Task ShouldReturnTokenWithInitializedUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageTypes.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(Access);
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>())).ReturnsAsync(new Token(AuthToken, RefreshToken));
      var handler =
        new SignInWithFacebookCommandHandler(InitializedDbContext(), _facebookService.Object, _tokenService.Object, _notifications.Object);

      var result = await handler.Handle(request);

      Assert.IsType<Token>(result);
    }

    [Fact]
    public async Task ShouldReturnTokenWithNotInitializedUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageTypes.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(Access);
      _facebookService
        .Setup(x => x.GetBody<dynamic>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync((object)GraphResponse());
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>())).ReturnsAsync(new Token(AuthToken, RefreshToken));
      var handler = new SignInWithFacebookCommandHandler(DbContext(), _facebookService.Object, _tokenService.Object, _notifications.Object);

      var result = await handler.Handle(request);

      Assert.IsType<Token>(result);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotVerifiedUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageTypes.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).Throws<UnauthorizedException>();
      var handler =
        new SignInWithFacebookCommandHandler(InitializedDbContext(), _facebookService.Object, _tokenService.Object, _notifications.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithRemovedUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageTypes.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(Access);
      var handler =
        new SignInWithFacebookCommandHandler(InitializedDbContextWithRemovedUser(), _facebookService.Object, _tokenService.Object, _notifications.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithDisabledUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageTypes.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(Access);
      var handler =
        new SignInWithFacebookCommandHandler(InitializedDbContextWithDisabledUser(), _facebookService.Object, _tokenService.Object, _notifications.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    private static SkelvyContext InitializedDbContextWithRemovedUser()
    {
      var context = DbContext();
      SkelvyInitializer.Initialize(context);
      var user = context.Users.FirstOrDefault(x => x.FacebookId == Access.UserId);

      if (user != null)
      {
        user.Remove(DateTimeOffset.UtcNow);
      }

      context.SaveChanges();

      return context;
    }

    private static SkelvyContext InitializedDbContextWithDisabledUser()
    {
      var context = DbContext();
      SkelvyInitializer.Initialize(context);
      var user = context.Users.FirstOrDefault(x => x.FacebookId == Access.UserId);

      if (user != null)
      {
        user.Disable("Test");
      }

      context.SaveChanges();

      return context;
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
