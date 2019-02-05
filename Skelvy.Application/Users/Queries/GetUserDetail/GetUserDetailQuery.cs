using MediatR;

namespace Skelvy.Application.Users.Queries.GetUserDetail
{
  public class GetUserDetailQuery : IRequest<UserDto>
  {
    public int Id { get; set; }
  }
}
