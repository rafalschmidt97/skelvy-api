using MediatR;

namespace Skelvy.Application.Users.Queries.FindUser
{
  public class FindUserQuery : IRequest<UserDto>
  {
    public int Id { get; set; }
  }
}
