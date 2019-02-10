using MediatR;

namespace Skelvy.Application.Users.Queries.GetUser
{
  public class GetUserQuery : IRequest<UserDto>
  {
    public int Id { get; set; }
  }
}
