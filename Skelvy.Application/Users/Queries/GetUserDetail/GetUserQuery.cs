using MediatR;

namespace Skelvy.Application.Users.Queries.GetUserDetail
{
  public class GetUserQuery : IRequest<UserDto>
  {
    public int Id { get; set; }
  }
}
