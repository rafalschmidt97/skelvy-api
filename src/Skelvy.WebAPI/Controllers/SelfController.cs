using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindSelf;

namespace Skelvy.WebAPI.Controllers
{
  public class SelfController : BaseController
  {
    [HttpGet("{id}")]
    public async Task<SelfViewModel> Find(int id)
    {
      return await Mediator.Send(new FindSelfQuery(id));
    }

    [HttpGet]
    public async Task<SelfViewModel> FindSelf()
    {
      return await Mediator.Send(new FindSelfQuery(UserId));
    }
  }
}
