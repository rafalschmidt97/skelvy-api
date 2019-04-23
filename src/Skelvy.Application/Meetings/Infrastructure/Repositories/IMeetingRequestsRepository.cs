using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingRequestsRepository : IBaseRepository
  {
    Task<MeetingRequest> FindOneWithDrinksByUserId(int userId);
    Task<IList<MeetingRequest>> FindAllByUsersId(IEnumerable<int> usersId);
  }
}
