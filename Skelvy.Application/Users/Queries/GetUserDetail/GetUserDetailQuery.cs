using MediatR;
using Skelvy.Application.Users.Queries.GetUsers;

namespace Skelvy.Application.Users.Queries.GetUserDetail
{
  public class GetUserDetailQuery : IRequest<UserDto>
  {
    public int Id { get; set; }
  }
}
