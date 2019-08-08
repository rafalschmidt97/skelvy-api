using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Infrastructure;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Relations.Queries.FindFriendRequests
{
  public class
    FindFriendRequestsQueryHandler : QueryHandler<FindFriendRequestsQuery, IList<FriendRequestDto>>
  {
    private readonly IRelationsRepository _relationsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public FindFriendRequestsQueryHandler(
      IRelationsRepository relationsRepository,
      IUsersRepository usersRepository,
      IMapper mapper)
    {
      _relationsRepository = relationsRepository;
      _usersRepository = usersRepository;
      _mapper = mapper;
    }

    public override async Task<IList<FriendRequestDto>> Handle(FindFriendRequestsQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var friendRequests = await _relationsRepository.FindAllFriendRequestsWithInvitingDetailsByUserId(request.UserId);
      return _mapper.Map<IList<FriendRequestDto>>(friendRequests);
    }
  }
}
