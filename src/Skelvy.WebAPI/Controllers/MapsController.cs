using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Infrastructure.Maps;
using Skelvy.Application.Maps.Queries.ReverseLocations;
using Skelvy.Application.Maps.Queries.SearchLocations;

namespace Skelvy.WebAPI.Controllers
{
  public class MapsController : BaseController
  {
    [HttpGet("search")]
    public async Task<IList<Location>> Search([FromQuery] SearchLocationsQuery request)
    {
      return await Mediator.Send(request, HttpContext.RequestAborted);
    }

    [HttpGet("reverse")]
    public async Task<IList<Location>> Reverse([FromQuery] ReverseLocationsQuery request)
    {
      return await Mediator.Send(request, HttpContext.RequestAborted);
    }
  }
}
