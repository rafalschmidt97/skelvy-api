using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Groups.Commands.LeaveGroup;
using Skelvy.Application.Groups.Commands.UpdateGroup;
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
  }
}
