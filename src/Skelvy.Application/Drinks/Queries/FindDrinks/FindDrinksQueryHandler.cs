using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Drinks.Infrastructure.Repositories;

namespace Skelvy.Application.Drinks.Queries.FindDrinks
{
  public class FindDrinksQueryHandler : QueryHandler<FindDrinksQuery, IList<DrinkDto>>
  {
    private readonly IDrinksRepository _repository;
    private readonly IMapper _mapper;

    public FindDrinksQueryHandler(IDrinksRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override async Task<IList<DrinkDto>> Handle(FindDrinksQuery request)
    {
      var drinks = await _repository.FindAll();
      return _mapper.Map<IList<DrinkDto>>(drinks);
    }
  }
}
