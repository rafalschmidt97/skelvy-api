using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Persistence;

namespace Skelvy.Application.Drinks.Queries.FindDrinks
{
  public class FindDrinksQueryHandler : IRequestHandler<FindDrinksQuery, IList<DrinkDto>>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public FindDrinksQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<IList<DrinkDto>> Handle(FindDrinksQuery request, CancellationToken cancellationToken)
    {
      return await _context.Drinks.ProjectTo<DrinkDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
  }
}
