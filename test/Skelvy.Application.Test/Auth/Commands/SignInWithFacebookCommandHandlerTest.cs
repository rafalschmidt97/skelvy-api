using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Commands.SignInWithFacebook;
using Skelvy.Application.Auth.Infrastructure.Facebook;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Auth.Commands
{
  public class SignInWithFacebookCommandHandlerTest : DatabaseRequestTestBase
  {
    private const string AuthToken = "Token";
    private const string AccessToken = "Token";
    private const string RefreshToken = "Token";
    private readonly AccessVerification _access;

    private readonly Mock<IFacebookService> _facebookService;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<SignInWithFacebookCommandHandler>> _logger;

    public SignInWithFacebookCommandHandlerTest()
    {
      _access = new AccessVerification("facebook1", AuthToken, DateTimeOffset.UtcNow.AddDays(3), AccessType.Facebook);
      _facebookService = new Mock<IFacebookService>();
      _tokenService = new Mock<ITokenService>();
      _mediator = new Mock<IMediator>();
      _logger = new Mock<ILogger<SignInWithFacebookCommandHandler>>();
    }

    [Fact]
    public async Task ShouldReturnTokenWithInitializedUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageType.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>()))
        .ReturnsAsync(new TokenDto { AccessToken = AccessToken, RefreshToken = RefreshToken });
      var dbContext = InitializedDbContext();
      var handler = new SignInWithFacebookCommandHandler(
          new UsersRepository(dbContext),
          new ProfilesRepository(dbContext),
          _facebookService.Object,
          _tokenService.Object,
          _mediator.Object,
          _logger.Object);

      var result = await handler.Handle(request);

      Assert.IsType<AuthDto>(result);
      Assert.False(result.AccountCreated);
    }

    [Fact]
    public async Task ShouldReturnTokenWithNotInitializedUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageType.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      _facebookService
        .Setup(x => x.GetBody<dynamic>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync((object)GraphResponse());
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>()))
        .ReturnsAsync(new TokenDto { AccessToken = AccessToken, RefreshToken = RefreshToken });
      var dbContext = DbContext();
      var handler = new SignInWithFacebookCommandHandler(
        new UsersRepository(dbContext),
        new ProfilesRepository(dbContext),
        _facebookService.Object,
        _tokenService.Object,
        _mediator.Object,
        _logger.Object);

      var result = await handler.Handle(request);

      Assert.IsType<AuthDto>(result);
      Assert.True(result.AccountCreated);
    }

    [Fact]
    public async Task ShouldThrowExceptionWithNotVerifiedUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageType.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).Throws<UnauthorizedException>();
      var dbContext = InitializedDbContext();
      var handler = new SignInWithFacebookCommandHandler(
        new UsersRepository(dbContext),
        new ProfilesRepository(dbContext),
        _facebookService.Object,
        _tokenService.Object,
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithTooLongEmail()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageType.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      var graphObject = GraphResponse();
      graphObject.email = "123456789123456789123456789123456789123456789@gmail.com"; // 55 signs
      _facebookService
        .Setup(x => x.GetBody<dynamic>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync((object)graphObject);
      _tokenService.Setup(x =>
          x.Generate(It.IsAny<User>()))
        .ReturnsAsync(new TokenDto { AccessToken = AccessToken, RefreshToken = RefreshToken });
      var dbContext = DbContext();
      var handler = new SignInWithFacebookCommandHandler(
        new UsersRepository(dbContext),
        new ProfilesRepository(dbContext),
        _facebookService.Object,
        _tokenService.Object,
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithRemovedUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageType.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      var dbContext = ContextWithRemovedUser();
      var handler = new SignInWithFacebookCommandHandler(
        new UsersRepository(dbContext),
        new ProfilesRepository(dbContext),
        _facebookService.Object,
        _tokenService.Object,
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithDisabledUser()
    {
      var request = new SignInWithFacebookCommand(AuthToken, LanguageType.EN);
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      var dbContext = ContextWithDisabledUser();
      var handler = new SignInWithFacebookCommandHandler(
        new UsersRepository(dbContext),
        new ProfilesRepository(dbContext),
        _facebookService.Object,
        _tokenService.Object,
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    private SkelvyContext ContextWithRemovedUser()
    {
      var context = DbContext();
      SkelvyInitializer.Initialize(context);
      var user = context.Users.FirstOrDefault(x => x.FacebookId == _access.UserId);

      if (user != null)
      {
        user.Remove(DateTimeOffset.UtcNow);
        context.Users.Update(user);
      }

      context.SaveChanges();

      return context;
    }

    private SkelvyContext ContextWithDisabledUser()
    {
      var context = DbContext();
      SkelvyInitializer.Initialize(context);
      var user = context.Users.FirstOrDefault(x => x.FacebookId == _access.UserId);

      if (user != null)
      {
        user.Disable("Test");
        context.Users.Update(user);
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
      return graphResponse;
    }
  }
}
