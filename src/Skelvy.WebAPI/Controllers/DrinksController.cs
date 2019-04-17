using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Drinks.Queries;
using Skelvy.Application.Drinks.Queries.FindDrinks;

namespace Skelvy.WebAPI.Controllers
{
  public class DrinksController : BaseController
  {
    [HttpGet]
    public async Task<IList<DrinkDto>> FindAll()
    {
      return await Mediator.Send(new FindDrinksQuery());
    }
  }
}
