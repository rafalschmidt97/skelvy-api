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
    [HttpPost]
    public async Task<ActionResult<ICollection<User>>> Add(CreateUserCommand request)
    {
      var userId = await Mediator.Send(request);
      return Ok(userId);
    }
  }
}
