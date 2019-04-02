using System.Collections.Generic;
using MediatR;

namespace Skelvy.Application.Drinks.Queries.FindDrinks
{
  public class FindDrinksQuery : IRequest<IList<DrinkDto>>
  {
  }
}
