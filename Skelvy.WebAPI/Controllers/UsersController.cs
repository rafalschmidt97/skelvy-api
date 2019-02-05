using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Users.Commands.CreateUser;
using Skelvy.Application.Users.Queries.GetUserDetail;
using Skelvy.Application.Users.Queries.GetUsers;
using Skelvy.Domain.Entities;

namespace Skelvy.WebAPI.Controllers
{
  public class UsersController : BaseController
  {
    [HttpGet]
    public async Task<ActionResult<ICollection<User>>> GetAll()
    {
      var users = await Mediator.Send(new GetUsersQuery());
      return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ICollection<User>>> Get(int id)
    {
      var user = await Mediator.Send(new GetUserDetailQuery { Id = id });
      return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<ICollection<User>>> Add(CreateUserCommand request)
    {
      var userId = await Mediator.Send(request);
      return Ok(userId);
    }
  }
}
