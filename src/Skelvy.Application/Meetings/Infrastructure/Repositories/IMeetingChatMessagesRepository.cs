using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingChatMessagesRepository : IBaseRepository
  {
    Task<IList<MeetingChatMessage>> FindPageByMeetingId(int meetingId, int page = 1, int pageSize = 20);
  }
}
