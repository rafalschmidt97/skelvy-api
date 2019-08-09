using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindSelf;

namespace Skelvy.WebAPI.Controllers
{
  public class SelfController : BaseController
  {
    [HttpGet]
    public async Task<SelfModel> FindSelf([FromQuery] FindSelfQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }
  }
}
