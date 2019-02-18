using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Drinks.Queries;
using Skelvy.Application.Drinks.Queries.GetDrinks;

namespace Skelvy.WebAPI.Controllers
{
  public class DrinksController : BaseController
  {
    [HttpGet]
    public async Task<ICollection<DrinkDto>> GetAll()
    {
      return await Mediator.Send(new GetDrinksQuery());
    }
  }
}
