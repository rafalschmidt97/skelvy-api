using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Queries.FIndUsers
{
  public class FindUsersQueryHandler : QueryHandler<FindUsersQuery, IList<UserWithRelationTypeDto>>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public FindUsersQueryHandler(IUsersRepository usersRepository, IMapper mapper)
    {
      _usersRepository = usersRepository;
      _mapper = mapper;
    }

    public override async Task<IList<UserWithRelationTypeDto>> Handle(FindUsersQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var users = await _usersRepository
        .FindPageWithRelationTypeByUserIdAndNameLike(request.UserId, request.UserName, request.Page);

      return _mapper.Map<IList<UserWithRelationTypeDto>>(users);
    }
  }
}
