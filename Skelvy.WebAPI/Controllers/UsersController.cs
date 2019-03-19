using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Users.Commands.RemoveUser;
using Skelvy.Application.Users.Commands.UpdateUserProfile;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindUser;

namespace Skelvy.WebAPI.Controllers
{
  public class UsersController : BaseController
  {
    [HttpGet("self")]
    public async Task<UserDto> FindSelf()
    {
      return await Mediator.Send(new FindUserQuery { Id = UserId });
    }

    [HttpGet("{id}")]
    public async Task<UserDto> Find(int id)
    {
      return await Mediator.Send(new FindUserQuery { Id = id });
    }

    [HttpPut("self/profile")]
    public async Task UpdateSelfProfile(UpdateUserProfileCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpDelete("self")]
    public async Task RemoveSelf()
    {
      await Mediator.Send(new RemoveUserCommand { Id = UserId });
    }
  }
}
