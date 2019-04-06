using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Users.Commands;
using Skelvy.Application.Users.Commands.DisableUser;
using Skelvy.Application.Users.Commands.RemoveUser;
using Skelvy.Application.Users.Commands.UpdateUserLanguage;
using Skelvy.Application.Users.Commands.UpdateUserProfile;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindUser;
using Skelvy.WebAPI.Filters;

namespace Skelvy.WebAPI.Controllers
{
  public class UsersController : BaseController
  {
    [HttpGet("self")]
    public async Task<UserDto> FindSelf()
    {
      return await Mediator.Send(new FindUserQuery { Id = UserId }, HttpContext.RequestAborted);
    }

    [HttpGet("{id}")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task<UserDto> Find(int id)
    {
      return await Mediator.Send(new FindUserQuery { Id = id }, HttpContext.RequestAborted);
    }

    [HttpDelete("{id}")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task Remove(int id)
    {
      await Mediator.Send(new RemoveUserCommand { Id = id }, HttpContext.RequestAborted);
    }

    [HttpPatch("{id}/disabled")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task Disable(int id, DisableUserCommand request)
    {
      request.Id = id;
      await Mediator.Send(request, HttpContext.RequestAborted);
    }

    [HttpPatch("self/language")]
    public async Task UpdateSelfLanguage(UpdateUserLanguageCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request, HttpContext.RequestAborted);
    }

    [HttpPut("self/profile")]
    public async Task UpdateSelfProfile(UpdateUserProfileCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request, HttpContext.RequestAborted);
    }
  }
}
