using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Users.Queries.GetUserDetail;

namespace Skelvy.WebAPI.Controllers
{
  public class UsersController : BaseController
  {
    [HttpGet("{id}")]
    public async Task<UserDto> Get(int id)
    {
      return await Mediator.Send(new GetUserQuery { Id = id });
    }
  }
}
