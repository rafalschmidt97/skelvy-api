using System.Threading.Tasks;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Infrastructure.Repositories;

namespace Skelvy.Application.Users.Queries.CheckUserName
{
  public class CheckUserNameQueryHandler : QueryHandler<CheckUserNameQuery, bool>
  {
    private readonly IUsersRepository _repository;

    public CheckUserNameQueryHandler(IUsersRepository repository)
    {
      _repository = repository;
    }

    public override async Task<bool> Handle(CheckUserNameQuery request)
    {
      var isNameNotAvailable = await _repository.ExistsOneWithRemovedByName(request.Name);
      return !isNameNotAvailable;
    }
  }
}
