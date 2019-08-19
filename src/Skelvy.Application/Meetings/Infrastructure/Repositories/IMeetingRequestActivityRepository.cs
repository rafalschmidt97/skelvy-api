using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingRequestActivityRepository : IBaseRepository
  {
    Task<IList<MeetingRequestActivity>> FindAllByRequestId(int requestId);
    Task<IList<MeetingRequestActivity>> FindAllByRequestsId(IEnumerable<int> requestsId);
    Task AddRange(IEnumerable<MeetingRequestActivity> activities);
    Task RemoveRange(IList<MeetingRequestActivity> activities);
  }
}
