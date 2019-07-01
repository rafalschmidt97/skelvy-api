using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class MeetingChatMessagesRepository : BaseRepository, IMeetingChatMessagesRepository
  {
    public MeetingChatMessagesRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<MeetingChatMessage>> FindPageBeforeByMeetingId(
      int meetingId,
      DateTimeOffset beforeDate,
      int pageSize = 20)
    {
      var messages = await Context.MeetingChatMessages
        .Where(x => x.MeetingId == meetingId && x.Date < beforeDate)
        .OrderByDescending(p => p.Date)
        .Take(pageSize)
        .ToListAsync();

      return messages.OrderBy(x => x.Date).ToList();
    }

    public async Task<IList<MeetingChatMessage>> FindPageLatestByMeetingId(int meetingId, int pageSize = 20)
    {
      var messages = await Context.MeetingChatMessages
        .Where(x => x.MeetingId == meetingId)
        .OrderByDescending(p => p.Date)
        .Take(pageSize)
        .ToListAsync();

      return messages.OrderBy(x => x.Date).ToList();
    }

    public async Task<IList<MeetingChatMessage>> FindAllByUsersId(IEnumerable<int> usersId)
    {
      return await Context.MeetingChatMessages
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }

    public async Task Add(MeetingChatMessage message)
    {
      await Context.MeetingChatMessages.AddAsync(message);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<MeetingChatMessage> messages)
    {
      Context.MeetingChatMessages.RemoveRange(messages);
      await SaveChanges();
    }
  }
}
