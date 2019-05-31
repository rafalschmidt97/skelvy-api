using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingRequestDrinkTypesRepository : IBaseRepository
  {
    Task<IList<MeetingRequestDrinkType>> FindAllByRequestsId(IEnumerable<int> requestsId);
    Task AddRange(IEnumerable<MeetingRequestDrinkType> drinkTypes);
    Task RemoveRange(IList<MeetingRequestDrinkType> drinkTypes);
  }
}
