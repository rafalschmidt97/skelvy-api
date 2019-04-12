using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Persistence;

namespace Skelvy.Application.Drinks.Queries.FindDrinks
{
  public class FindDrinksQueryHandler : QueryHandler<FindDrinksQuery, IList<DrinkDto>>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public FindDrinksQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public override async Task<IList<DrinkDto>> Handle(FindDrinksQuery request)
    {
      return await _context.Drinks.ProjectTo<DrinkDto>(_mapper.ConfigurationProvider).ToListAsync();
    }
  }
}
