using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.LeaveGroup;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindGroups;

namespace Skelvy.WebAPI.Controllers
{
  public class GroupsController : BaseController
  {
    [HttpPost("{id}/leave")]
    public async Task LeaveSelf(int id)
    {
      await Mediator.Send(new LeaveGroupCommand(id, UserId));
    }

    [HttpGet("self")]
    public async Task<GroupsModel> FindAllSelfGroups([FromQuery] FindGroupsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }
  }
}
