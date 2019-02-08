using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Queries.GetUserDetail
{
  public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
  {
    private readonly SkelvyContext _context;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(SkelvyContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
      var user = await _context.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(value => value.Id == request.Id, cancellationToken);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.Id);
      }

      return user;
    }
  }
}
