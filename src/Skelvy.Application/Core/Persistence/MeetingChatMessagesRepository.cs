using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Core.Persistence
{
  public class MeetingChatMessagesRepository : BaseRepository, IMeetingChatMessagesRepository
  {
    public MeetingChatMessagesRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<MeetingChatMessage>> FindPageByMeetingId(int meetingId, int page = 1, int pageSize = 20)
    {
      var skip = (page - 1) * pageSize;
      var messages = await Context.MeetingChatMessages
        .OrderByDescending(p => p.Date)
        .Skip(skip)
        .Take(pageSize)
        .Where(x => x.MeetingId == meetingId)
        .ToListAsync();

      return messages.OrderBy(x => x.Date).ToList();
    }
  }
}
