using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Skelvy.Application.Core.Pipes;
using Skelvy.Application.Users.Commands.CreateUser;
using Xunit;

namespace Skelvy.Application.Test.Core.Pipes
{
  public class RequestValidationTest
  {
    private const string UserEmail = "user@gmail.com";
    private const string UserName = "User";

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new CreateUserCommand { Email = UserEmail };
      var validators = new List<IValidator<CreateUserCommand>> { new CreateUserCommandValidator() };
      var pipe = new RequestValidation<CreateUserCommand>(validators);

      await Assert.ThrowsAsync<ValidationException>(() =>
        pipe.Process(request, CancellationToken.None));
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new CreateUserCommand { Email = UserEmail, Name = UserName };
      var validators = new List<IValidator<CreateUserCommand>> { new CreateUserCommandValidator() };
      var pipe = new RequestValidation<CreateUserCommand>(validators);

      await pipe.Process(request, CancellationToken.None);
    }
  }
}
