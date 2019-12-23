using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Relations.Queries.FindRelation
{
  public class FindRelationQueryHandler : QueryHandler<FindRelationQuery, RelationDto>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IRelationsRepository _relationsRepository;
    private readonly IMapper _mapper;

    public FindRelationQueryHandler(IUsersRepository usersRepository, IRelationsRepository relationsRepository, IMapper mapper)
    {
      _usersRepository = usersRepository;
      _relationsRepository = relationsRepository;
      _mapper = mapper;
    }

    public override async Task<RelationDto> Handle(FindRelationQuery request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var relatedUserExists = await _usersRepository.ExistsOne(request.RelatedUserId);

      if (!relatedUserExists)
      {
        throw new NotFoundException(nameof(User), request.RelatedUserId);
      }

      var relation = await _relationsRepository.FindOneByUserIdAndRelatedUserIdTwoWay(request.UserId, request.RelatedUserId);

      if (relation == null)
      {
        throw new NotFoundException(nameof(Relation), $"(UserId = {request.UserId}, RelatedUserIdd = {request.RelatedUserId})");
      }

      return _mapper.Map<RelationDto>(relation);
    }
  }
}
