using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Infrastructure.Repositories
{
  public interface IMeetingChatMessagesRepository : IBaseRepository
  {
    Task<IList<MeetingChatMessage>> FindPageBeforeByMeetingId(int meetingId, DateTimeOffset beforeDate, int pageSize = 20);
    Task<IList<MeetingChatMessage>> FindPageLatestByMeetingId(int meetingId, int pageSize = 20);
    Task<IList<MeetingChatMessage>> FindAllByUsersId(IEnumerable<int> usersId);
    Task Add(MeetingChatMessage message);
    Task RemoveRange(IList<MeetingChatMessage> messages);
  }
}
