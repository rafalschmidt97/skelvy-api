using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Infrastructure;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Application.Users.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Relations.Queries.FIndFriends
{
  public class FindFriendsQueryHandler : QueryHandler<FindFriendsQuery, IList<UserDto>>
  {
    private readonly IRelationsRepository _relationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public FindFriendsQueryHandler(
      IRelationsRepository relationsRepository,
      IMapper mapper,
      IUsersRepository usersRepository)
    {
      _relationsRepository = relationsRepository;
      _mapper = mapper;
      _usersRepository = usersRepository;
    }

    public override async Task<IList<UserDto>> Handle(FindFriendsQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var friendRelations = await _relationsRepository
        .FindPageRelationsUsersWithRelatedDetailsByUserIdAndType(request.UserId, RelationType.Friend, request.Page);

      return _mapper.Map<IList<UserDto>>(friendRelations);
    }
  }
}
