using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Queries.FindUser
{
  public class FindUserQueryHandler : QueryHandler<FindUserQuery, UserDto>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public FindUserQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public override async Task<UserDto> Handle(FindUserQuery request)
    {
      var user = await _context.Users
        .Include(x => x.Profile)
        .ThenInclude(x => x.Photos)
        .FirstOrDefaultAsync(x => x.Id == request.Id);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.Id);
      }

      return _mapper.Map<UserDto>(user);
    }
  }
}
