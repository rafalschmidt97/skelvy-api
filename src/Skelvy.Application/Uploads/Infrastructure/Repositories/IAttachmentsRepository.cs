using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Persistence;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Uploads.Infrastructure.Repositories
{
  public interface IAttachmentsRepository : IBaseRepository
  {
    Task<Attachment> FindOneByAttachmentId(int id);
    Task<IList<Attachment>> FindAllByAttachmentsId(IEnumerable<int> ids);
    Task Add(Attachment attachment);
    Task AddRange(IList<Attachment> attachments);
    Task Remove(Attachment attachment);
    Task RemoveRange(IList<Attachment> attachments);
  }
}
