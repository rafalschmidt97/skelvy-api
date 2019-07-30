using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class MessagesRepository : BaseRepository, IMessagesRepository
  {
    public MessagesRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<IList<Message>> FindPageBeforeByMeetingId(
      int meetingId,
      DateTimeOffset beforeDate,
      int pageSize = 20)
    {
      var messages = await Context.Messages
        .Where(x => x.GroupId == meetingId && x.Date < beforeDate)
        .OrderByDescending(p => p.Date)
        .Take(pageSize)
        .ToListAsync();

      return messages.OrderBy(x => x.Date).ToList();
    }

    public async Task<IList<Message>> FindPageLatestByMeetingId(int meetingId, int pageSize = 20)
    {
      var messages = await Context.Messages
        .Where(x => x.GroupId == meetingId)
        .OrderByDescending(p => p.Date)
        .Take(pageSize)
        .ToListAsync();

      return messages.OrderBy(x => x.Date).ToList();
    }

    public async Task<IList<Message>> FindAllByUsersId(IEnumerable<int> usersId)
    {
      return await Context.Messages
        .Where(x => usersId.Any(y => y == x.UserId))
        .ToListAsync();
    }

    public async Task Add(Message message)
    {
      await Context.Messages.AddAsync(message);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<Message> messages)
    {
      Context.Messages.RemoveRange(messages);
      await SaveChanges();
    }
  }
}
