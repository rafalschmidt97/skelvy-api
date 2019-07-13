using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Queries.FindSelfUser
{
  public class FindSelfUserQueryHandler : QueryHandler<FindSelfUserQuery, SelfUserDto>
  {
    private readonly IUsersRepository _repository;
    private readonly IMapper _mapper;

    public FindSelfUserQueryHandler(IUsersRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override async Task<SelfUserDto> Handle(FindSelfUserQuery request)
    {
      var user = await _repository.FindOneWithDetails(request.Id);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.Id);
      }

      return _mapper.Map<SelfUserDto>(user);
    }
  }
}
