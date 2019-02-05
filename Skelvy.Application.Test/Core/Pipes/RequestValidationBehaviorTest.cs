using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Moq;
using Skelvy.Application.Core.Pipes;
using Skelvy.Application.Users.Commands.CreateUser;
using Xunit;

namespace Skelvy.Application.Test.Core.Pipes
{
  public class RequestValidationBehaviorTest
  {
    private const string UserEmail = "user@gmail.com";
    private const string UserName = "User";

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new CreateUserCommand { Email = UserEmail };
      var validators = new List<IValidator<CreateUserCommand>> { new CreateUserCommandValidator() };
      var behavior = new RequestValidationBehavior<CreateUserCommand, int>(validators);

      await Assert.ThrowsAsync<ValidationException>(() =>
        behavior.Handle(request, CancellationToken.None, null));
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new CreateUserCommand { Email = UserEmail, Name = UserName };
      var validators = new List<IValidator<CreateUserCommand>> { new CreateUserCommandValidator() };
      var behavior = new RequestValidationBehavior<CreateUserCommand, int>(validators);
      var next = new Mock<RequestHandlerDelegate<int>>();

      await behavior.Handle(request, CancellationToken.None, next.Object);
    }
  }
}
