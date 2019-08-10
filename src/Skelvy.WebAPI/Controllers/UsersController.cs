using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Users.Commands.DisableUser;
using Skelvy.Application.Users.Commands.RemoveUser;
using Skelvy.Application.Users.Commands.UpdateProfile;
using Skelvy.Application.Users.Commands.UpdateUserLanguage;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindSelfUser;
using Skelvy.Application.Users.Queries.FindUser;
using Skelvy.Domain.Enums.Users;
using Skelvy.WebAPI.Filters;

namespace Skelvy.WebAPI.Controllers
{
  public class UsersController : BaseController
  {
    [HttpGet("{id}")]
    public async Task<UserDto> Find(int id)
    {
      return await Mediator.Send(new FindUserQuery(id));
    }

    [HttpGet("self")]
    public async Task<SelfUserDto> FindSelf()
    {
      return await Mediator.Send(new FindSelfUserQuery(UserId));
    }

    [HttpPatch("self/language")]
    public async Task UpdateSelfLanguage(UpdateUserLanguageCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPut("self/profile")]
    public async Task UpdateSelfProfile(UpdateProfileCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpDelete("{id}")]
    [AuthorizeRole(RoleType.Admin)]
    public async Task Remove(int id)
    {
      await Mediator.Send(new RemoveUserCommand(id));
    }

    [HttpPatch("{id}/disabled")]
    [AuthorizeRole(RoleType.Admin)]
    public async Task Disable(int id, DisableUserCommand request)
    {
      request.Id = id;
      await Mediator.Send(request);
    }
  }
}
