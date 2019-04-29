using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingRequestDrinksRepository : IBaseRepository
  {
    Task<IList<MeetingRequestDrink>> FindAllByRequestsId(IEnumerable<int> requestsId);
    Task AddRange(IEnumerable<MeetingRequestDrink> drinks);
    Task RemoveRange(IList<MeetingRequestDrink> drinks);
  }
}
