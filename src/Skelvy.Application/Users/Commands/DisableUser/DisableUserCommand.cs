using MediatR;

namespace Skelvy.Application.Users.Commands.DisableUser
{
  public class DisableUserCommand : IRequest
  {
    public int Id { get; set; }
    public string Reason { get; set; }
  }
}
