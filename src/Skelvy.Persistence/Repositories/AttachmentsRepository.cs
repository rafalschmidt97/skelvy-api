using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Uploads.Infrastructure.Repositories;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Repositories
{
  public class AttachmentsRepository : BaseRepository, IAttachmentsRepository
  {
    public AttachmentsRepository(SkelvyContext context)
      : base(context)
    {
    }

    public async Task<Attachment> FindOneByAttachmentId(int id)
    {
      return await Context.Attachments.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IList<Attachment>> FindAllByAttachmentsId(IEnumerable<int> ids)
    {
      return await Context.Attachments.Where(x => ids.Any(y => y == x.Id)).ToListAsync();
    }

    public async Task Add(Attachment attachment)
    {
      await Context.Attachments.AddAsync(attachment);
      await SaveChanges();
    }

    public async Task AddRange(IList<Attachment> attachments)
    {
      await Context.Attachments.AddRangeAsync(attachments);
      await SaveChanges();
    }

    public async Task Remove(Attachment attachment)
    {
      Context.Attachments.Remove(attachment);
      await SaveChanges();
    }

    public async Task RemoveRange(IList<Attachment> attachments)
    {
      Context.Attachments.RemoveRange(attachments);
      await SaveChanges();
    }
  }
}
