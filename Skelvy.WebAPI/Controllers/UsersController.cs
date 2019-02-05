using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Users.Commands.CreateUser;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.GetUserDetail;
using Skelvy.Application.Users.Queries.GetUsers;

namespace Skelvy.WebAPI.Controllers
{
  public class UsersController : BaseController
  {
    [HttpGet]
    public async Task<ICollection<UserDto>> GetAll()
    {
      return await Mediator.Send(new GetUsersQuery());
    }

    [HttpGet("{id}")]
    public async Task<UserDto> Get(int id)
    {
      return await Mediator.Send(new GetUserDetailQuery { Id = id });
    }

    [HttpPost]
    public async Task<double> Add(CreateUserCommand request)
    {
      return await Mediator.Send(request);
    }
  }
}
