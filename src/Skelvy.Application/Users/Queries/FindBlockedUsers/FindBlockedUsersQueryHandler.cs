using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Queries.FindBlockedUsers
{
  public class FindBlockedUsersQueryHandler : QueryHandler<FindBlockedUsersQuery, IList<UserDto>>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IBlockedUsersRepository _blockedUsersRepository;
    private readonly IMapper _mapper;

    public FindBlockedUsersQueryHandler(IUsersRepository usersRepository, IBlockedUsersRepository blockedUsersRepository, IMapper mapper)
    {
      _usersRepository = usersRepository;
      _blockedUsersRepository = blockedUsersRepository;
      _mapper = mapper;
    }

    public override async Task<IList<UserDto>> Handle(FindBlockedUsersQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException($"Entity {nameof(User)}(Id = {request.UserId}) not found.");
      }

      var blockedUsers = await _blockedUsersRepository.FindPageWithDetailsByUserId(request.UserId, request.Page);
      return _mapper.Map<IList<UserDto>>(blockedUsers);
    }
  }
}
