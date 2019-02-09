using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Auth.Commands;
using Skelvy.Application.Auth.Commands.SignInWithFacebook;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Core.Infrastructure.Facebook;
using Skelvy.Application.Core.Infrastructure.Tokens;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;
using Xunit;

namespace Skelvy.Application.Test.Auth.Commands
{
  public class SignInWithFacebookCommandHandlerTest : RequestTestBase
  {
    private const string AuthToken = "Token";
    private static User _user;
    private static AccessVerification _access;

    private readonly Mock<IFacebookService> _facebookService;
    private readonly Mock<ITokenService> _tokenService;

    public SignInWithFacebookCommandHandlerTest()
    {
      _facebookService = new Mock<IFacebookService>();
      _tokenService = new Mock<ITokenService>();

      _user = new User
      {
        Id = 1,
        Email = "user@gmail.com",
        FacebookId = "1"
      };

      _access = new AccessVerification
      {
        UserId = _user.FacebookId
      };
    }

    [Fact]
    public async Task ShouldReturnTokenWithInitializedUser()
    {
      var request = new SignInWithFacebookCommand { AuthToken = AuthToken };
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>(), It.IsAny<AccessVerification>())).Returns("abc");
      var handler =
        new SignInWithFacebookCommandHandler(InitializedDbContext(), _facebookService.Object, _tokenService.Object);

      var result = await handler.Handle(request, CancellationToken.None);

      Assert.IsType<string>(result);
    }

    [Fact]
    public async Task ShouldReturnTokenWithNotInitializedUser()
    {
      var request = new SignInWithFacebookCommand { AuthToken = AuthToken };
      _facebookService.Setup(x => x.Verify(It.IsAny<string>())).ReturnsAsync(_access);
      dynamic emailResponse = new ExpandoObject();
      emailResponse.email = _user.Email;
      _facebookService
        .Setup(x => x.GetBody<dynamic>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync((object)emailResponse);
      _tokenService.Setup(x =>
        x.Generate(It.IsAny<User>(), It.IsAny<AccessVerification>())).Returns("abc");
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

    private static SkelvyContext InitializedDbContext()
    {
      var context = DbContext();
      context.Users.Add(_user);
      context.SaveChanges();
      return context;
    }
  }
}
