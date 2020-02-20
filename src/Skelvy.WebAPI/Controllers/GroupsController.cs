using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Groups.Commands.LeaveGroup;
using Skelvy.Application.Groups.Commands.RemoveUserFromGroup;
using Skelvy.Application.Groups.Commands.UpdateGroup;
using Skelvy.Application.Groups.Commands.UpdateGroupUserRole;
using Skelvy.Application.Groups.Queries.FindGroup;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.WebAPI.Controllers
{
  public class GroupsController : BaseController
  {
    [HttpGet("{id}")]
    public async Task<GroupDto> Find(int id)
    {
      return await Mediator.Send(new FindGroupQuery(id, UserId));
    }

    [HttpPost("{id}/leave")]
    public async Task Leave(int id)
    {
      await Mediator.Send(new LeaveGroupCommand(id, UserId));
    }

    [HttpPut("{id}")]
    public async Task Update(int id, UpdateGroupCommand request)
    {
      request.GroupId = id;
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpDelete("{id}/users/{userId}")]
    public async Task RemoveUser(int id, int userId)
    {
      await Mediator.Send(new RemoveUserFromGroupCommand(UserId, id, userId));
    }

    [HttpPatch("{id}/users/{userId}/role")]
    public async Task UpdateUserRole(int id, int userId, UpdateGroupUserRoleCommand request)
    {
      request.UserId = UserId;
      request.GroupId = id;
      request.UpdatedUserId = userId;
      await Mediator.Send(request);
    }
  }
}
