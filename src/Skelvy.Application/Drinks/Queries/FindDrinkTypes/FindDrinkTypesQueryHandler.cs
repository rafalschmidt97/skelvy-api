using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Drinks.Infrastructure.Repositories;

namespace Skelvy.Application.Drinks.Queries.FindDrinkTypes
{
  public class FindDrinkTypesQueryHandler : QueryHandler<FindDrinkTypesQuery, IList<DrinkTypeDto>>
  {
    private readonly IDrinkTypesRepository _repository;
    private readonly IMapper _mapper;

    public FindDrinkTypesQueryHandler(IDrinkTypesRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override async Task<IList<DrinkTypeDto>> Handle(FindDrinkTypesQuery request)
    {
      var drinkTypes = await _repository.FindAll();
      return _mapper.Map<IList<DrinkTypeDto>>(drinkTypes);
    }
  }
}
