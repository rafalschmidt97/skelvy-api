using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Persistence;

namespace Skelvy.Application.Drinks.Queries.GetDrinks
{
  public class GetDrinksQueryHandler : IRequestHandler<GetDrinksQuery, ICollection<DrinkDto>>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public GetDrinksQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ICollection<DrinkDto>> Handle(GetDrinksQuery request, CancellationToken cancellationToken)
    {
      var drinks = await _context.Drinks.ToListAsync(cancellationToken);
      return _mapper.Map<ICollection<DrinkDto>>(drinks);
    }
  }
}
