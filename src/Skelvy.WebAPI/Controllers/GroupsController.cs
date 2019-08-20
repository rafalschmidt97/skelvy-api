using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Groups.Commands.LeaveGroup;

namespace Skelvy.WebAPI.Controllers
{
  public class GroupsController : BaseController
  {
    [HttpPost("{id}/leave")]
    public async Task Leave(int id)
    {
      await Mediator.Send(new LeaveGroupCommand(id, UserId));
    }
  }
}
