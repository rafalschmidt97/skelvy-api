using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Core.Infrastructure.Notifications
{
  public interface INotificationsService
  {
    Task SendMessage(MeetingChatMessage message, CancellationToken cancellationToken);
    Task SendMessages(ICollection<MeetingChatMessageDto> messages, int userid, CancellationToken cancellationToken);
  }
}
