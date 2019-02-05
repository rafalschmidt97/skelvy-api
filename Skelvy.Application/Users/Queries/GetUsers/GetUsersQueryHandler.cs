using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Queries.GetUsers
{
  public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ICollection<UserDto>>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<ICollection<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
      return await _context.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
      // return _mapper.Map<ICollection<User>, ICollection<UserDto>>(users);
    }
  }
}
