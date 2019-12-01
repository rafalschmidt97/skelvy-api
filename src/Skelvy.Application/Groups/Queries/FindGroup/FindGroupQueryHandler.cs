using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Groups.Queries.FindGroup
{
  public class FindGroupQueryHandler : QueryHandler<FindGroupQuery, GroupDto>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IGroupsRepository _groupsRepository;
    private readonly IMapper _mapper;

    public FindGroupQueryHandler(
      IUsersRepository usersRepository,
      IGroupsRepository groupsRepository,
      IMapper mapper)
    {
      _usersRepository = usersRepository;
      _groupsRepository = groupsRepository;
      _mapper = mapper;
    }

    public override async Task<GroupDto> Handle(FindGroupQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var group = await _groupsRepository
        .FindOneWithUsersDetailsAndMessagesByGroupIdAndUserId(request.GroupId, request.UserId);

      return _mapper.Map<GroupDto>(group);
    }
  }
}
