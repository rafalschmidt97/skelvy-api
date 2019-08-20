using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Queries.FindUser
{
  public class FindUserQueryHandler : QueryHandler<FindUserQuery, UserDto>
  {
    private readonly IUsersRepository _repository;
    private readonly IMapper _mapper;

    public FindUserQueryHandler(IUsersRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override async Task<UserDto> Handle(FindUserQuery request)
    {
      var user = await _repository.FindOneWithDetails(request.UserId);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      return _mapper.Map<UserDto>(user);
    }
  }
}
