using System.Collections.Generic;
using MediatR;

namespace Skelvy.Application.Drinks.Queries.GetDrinks
{
  public class GetDrinksQuery : IRequest<ICollection<DrinkDto>>
  {
  }
}
