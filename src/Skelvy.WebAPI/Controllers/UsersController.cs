using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Users.Commands.DisableUser;
using Skelvy.Application.Users.Commands.RemoveUser;
using Skelvy.Application.Users.Commands.SendEmailToUser;
using Skelvy.Application.Users.Commands.SendEmailToUsers;
using Skelvy.Application.Users.Commands.UpdateProfile;
using Skelvy.Application.Users.Commands.UpdateUserEmail;
using Skelvy.Application.Users.Commands.UpdateUserLanguage;
using Skelvy.Application.Users.Commands.UpdateUserName;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.CheckUserName;
using Skelvy.Application.Users.Queries.FindSelfUser;
using Skelvy.Application.Users.Queries.FindUser;
using Skelvy.Application.Users.Queries.FIndUsers;
using Skelvy.Application.Users.Queries.Sync;
using Skelvy.Domain.Enums;
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

    [HttpGet]
    public async Task<IList<UserDto>> FindAll([FromQuery] FindUsersQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpPatch("self/language")]
    public async Task UpdateSelfLanguage(UpdateUserLanguageCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpGet("name-available")]
    public async Task<bool> CheckUserName([FromQuery] CheckUserNameQuery request)
    {
      return await Mediator.Send(request);
    }

    [HttpPatch("self/name")]
    public async Task UpdateSelfName(UpdateUserNameCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPatch("self/email")]
    public async Task UpdateSelfEmail(UpdateUserEmailCommand request)
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

    [HttpGet("self/sync")]
    public async Task<SyncModel> SyncSelf([FromQuery] SyncQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpDelete("{id}")]
    [AuthorizeRole(RoleType.Admin)]
    public async Task Remove(int id)
    {
      await Mediator.Send(new RemoveUserCommand(id));
    }

    [HttpPatch("{id}/disable")]
    [AuthorizeRole(RoleType.Admin)]
    public async Task Disable(int id, DisableUserCommand request)
    {
      request.UserId = id;
      await Mediator.Send(request);
    }

    [HttpPost("send/list")]
    [AuthorizeRole(RoleType.Admin)]
    public async Task SendListEmail(SendEmailToUserCommand command)
    {
      await Mediator.Send(command);
    }

    [HttpPost("send/page")]
    [AuthorizeRole(RoleType.Admin)]
    public async Task SendPageEmail([FromQuery] int minId, [FromQuery] int maxId, SendEmailToUsersCommand command)
    {
      command.MinId = minId;
      command.MaxId = maxId;
      await Mediator.Send(command);
    }
  }
}
