using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Users.Commands.DeleteUser;
using Skelvy.Application.Users.Commands.UpdateUserProfile;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.GetUser;

namespace Skelvy.WebAPI.Controllers
{
  public class UsersController : BaseController
  {
    [HttpGet("self")]
    public async Task<UserDto> GetSelf()
    {
      return await Mediator.Send(new GetUserQuery { Id = UserId });
    }

    [HttpGet("{id}")]
    public async Task<UserDto> Get(int id)
    {
      return await Mediator.Send(new GetUserQuery { Id = id });
    }

    [HttpPut("self/profile")]
    public async Task UpdateSelfProfile(UpdateUserProfileCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpDelete("self")]
    public async Task DeleteSelf()
    {
      await Mediator.Send(new DeleteUserCommand { Id = UserId });
    }
  }
}
