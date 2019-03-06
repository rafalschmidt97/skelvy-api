using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Queries.FindUser
{
  public class FindUserQueryHandler : IRequestHandler<FindUserQuery, UserDto>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public FindUserQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<UserDto> Handle(FindUserQuery request, CancellationToken cancellationToken)
    {
      var user = await _context.Users
        .Include(x => x.Profile)
        .ThenInclude(x => x.Photos)
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.Id);
      }

      return _mapper.Map<UserDto>(user);
    }
  }
}