using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Commands.SignInWithGoogle;
using Skelvy.Application.Auth.Infrastructure.Google;
using Skelvy.Application.Auth.Infrastructure.Tokens;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;
using Skelvy.Persistence;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Auth.Commands
{
  public class SignInWithGoogleCommandHandlerTest : DatabaseRequestTestBase
  {
    private const string AuthToken = "Token";
    private const string AccessToken = "Token";
    private const string RefreshToken = "Token";
    private readonly AccessVerification _access;

    private readonly Mock<IGoogleService> _googleService;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<ILogger<SignInWithGoogleCommandHandler>> _logger;

    public SignInWithGoogleCommandHandlerTest()
    {
      _access = new AccessVerification("google1", AuthToken, DateTimeOffset.UtcNow.AddDays(3), AccessType.Google);
      _googleService = new Mock<IGoogleService>();
      _tokenService = new Mock<ITokenService>();
      _mediator = new Mock<IMediator>();
      _logger = new Mock<ILogger<SignInWithGoogleCommandHandler>>();
    }

    [Fact]
    public async Task ShouldReturnTokenWithInitializedUser()
    {
      var request = new SignInWithGoogleCommand(AuthToken, LanguageType.EN);
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>()))
        .ReturnsAsync(new TokenDto { AccessToken = AccessToken, RefreshToken = RefreshToken });
      var dbContext = InitializedDbContext();
      var handler = new SignInWithGoogleCommandHandler(
        new UsersRepository(dbContext),
        new ProfilesRepository(dbContext),
        _googleService.Object,
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
      var request = new SignInWithGoogleCommand(AuthToken, LanguageType.EN);
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      _googleService
        .Setup(x => x.GetBody<dynamic>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync((object)PeopleResponse());
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>()))
        .ReturnsAsync(new TokenDto { AccessToken = AccessToken, RefreshToken = RefreshToken });
      var dbContext = DbContext();
      var handler = new SignInWithGoogleCommandHandler(
        new UsersRepository(dbContext),
        new ProfilesRepository(dbContext),
        _googleService.Object,
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
      var request = new SignInWithGoogleCommand(AuthToken, LanguageType.EN);
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).Throws<UnauthorizedException>();
      var dbContext = InitializedDbContext();
      var handler = new SignInWithGoogleCommandHandler(
        new UsersRepository(dbContext),
        new ProfilesRepository(dbContext),
        _googleService.Object,
        _tokenService.Object,
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithTooLongEmail()
    {
      var request = new SignInWithGoogleCommand(AuthToken, LanguageType.EN);
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      var peopleResponse = PeopleResponse();
      peopleResponse.emails[0].value = "123456789123456789123456789123456789123456789@gmail.com"; // 55 signs
      _googleService
        .Setup(x => x.GetBody<dynamic>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync((object)peopleResponse);
      _tokenService.Setup(x =>
          x.Generate(It.IsAny<User>()))
        .ReturnsAsync(new TokenDto { AccessToken = AccessToken, RefreshToken = RefreshToken });
      var dbContext = DbContext();
      var handler = new SignInWithGoogleCommandHandler(
        new UsersRepository(dbContext),
        new ProfilesRepository(dbContext),
        _googleService.Object,
        _tokenService.Object,
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithRemovedUser()
    {
      var request = new SignInWithGoogleCommand(AuthToken, LanguageType.EN);
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).Throws<UnauthorizedException>();
      var dbContext = ContextWithRemovedUser();
      var handler = new SignInWithGoogleCommandHandler(
        new UsersRepository(dbContext),
        new ProfilesRepository(dbContext),
        _googleService.Object,
        _tokenService.Object,
        _mediator.Object,
        _logger.Object);

      await Assert.ThrowsAsync<UnauthorizedException>(() =>
        handler.Handle(request));
    }

    [Fact]
    public async Task ShouldThrowExceptionWithDisabledUser()
    {
      var request = new SignInWithGoogleCommand(AuthToken, LanguageType.EN);
      _googleService.Setup(x => x.Verify(It.IsAny<string>())).Throws<UnauthorizedException>();
      var dbContext = ContextWithDisabledUser();
      var handler = new SignInWithGoogleCommandHandler(
        new UsersRepository(dbContext),
        new ProfilesRepository(dbContext),
        _googleService.Object,
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
      return graphResponse;
    }
  }
}
