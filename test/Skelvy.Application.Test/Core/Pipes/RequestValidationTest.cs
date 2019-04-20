using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Skelvy.Application.Auth.Commands.SignInWithFacebook;
using Skelvy.Application.Core.Pipes;
using Skelvy.Domain.Enums.Users;
using Xunit;

namespace Skelvy.Application.Test.Core.Pipes
{
  public class RequestValidationTest
  {
    private const string Token = "Token";

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new SignInWithFacebookCommand();
      var validators = new List<IValidator<SignInWithFacebookCommand>> { new SignInWithFacebookCommandValidator() };
      var pipe = new RequestValidation<SignInWithFacebookCommand>(validators);

      await Assert.ThrowsAsync<ValidationException>(() =>
        pipe.Process(request, CancellationToken.None));
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new SignInWithFacebookCommand { AuthToken = Token, Language = LanguageTypes.EN };
      var validators = new List<IValidator<SignInWithFacebookCommand>> { new SignInWithFacebookCommandValidator() };
      var pipe = new RequestValidation<SignInWithFacebookCommand>(validators);

      await pipe.Process(request, CancellationToken.None);
    }
  }
}
