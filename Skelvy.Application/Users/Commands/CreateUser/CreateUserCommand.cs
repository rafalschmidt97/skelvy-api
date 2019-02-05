using MediatR;

namespace Skelvy.Application.Users.Commands.CreateUser
{
  public class CreateUserCommand : IRequest<int>
  {
    public string Email { get; set; }
    public string Name { get; set; }
  }
}
