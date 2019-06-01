using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Drinks.Queries;
using Skelvy.Application.Drinks.Queries.FindDrinkTypes;

namespace Skelvy.WebAPI.Controllers
{
  public class DrinksController : BaseController
  {
    [HttpGet("types")]
    public async Task<IList<DrinkTypeDto>> FindAll()
    {
      return await Mediator.Send(new FindDrinkTypesQuery());
    }
  }
}
