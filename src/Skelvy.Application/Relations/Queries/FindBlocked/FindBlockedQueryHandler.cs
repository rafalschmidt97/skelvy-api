using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Application.Users.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Relations.Queries.FindBlocked
{
  public class FindBlockedQueryHandler : QueryHandler<FindBlockedQuery, IList<UserDto>>
  {
    private readonly IRelationsRepository _relationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public FindBlockedQueryHandler(IRelationsRepository relationsRepository, IUsersRepository usersRepository, IMapper mapper)
    {
      _relationsRepository = relationsRepository;
      _usersRepository = usersRepository;
      _mapper = mapper;
    }

    public override async Task<IList<UserDto>> Handle(FindBlockedQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var blockedRelations = await _relationsRepository
        .FindPageUsersWithRelatedDetailsByUserIdAndType(request.UserId, RelationType.Blocked, request.Page);

      return _mapper.Map<IList<UserDto>>(blockedRelations);
    }
  }
}
