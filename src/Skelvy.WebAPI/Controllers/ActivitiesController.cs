using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Activities.Queries;
using Skelvy.Application.Activities.Queries.FindActivities;

namespace Skelvy.WebAPI.Controllers
{
  public class ActivitiesController : BaseController
  {
    [HttpGet]
    public async Task<IList<ActivityDto>> FindAll([FromQuery] FindActivitiesQuery request)
    {
      return await Mediator.Send(request);
    }
  }
}
