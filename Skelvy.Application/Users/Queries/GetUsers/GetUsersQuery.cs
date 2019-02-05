using System.Collections.Generic;
using MediatR;

namespace Skelvy.Application.Users.Queries.GetUsers
{
  public class GetUsersQuery : IRequest<ICollection<UserDto>>
  {
  }
}
