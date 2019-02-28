using MediatR;

namespace Skelvy.Application.Users.Commands.RemoveUser
{
  public class RemoveUserCommand : IRequest
  {
    public int Id { get; set; }
  }
}
