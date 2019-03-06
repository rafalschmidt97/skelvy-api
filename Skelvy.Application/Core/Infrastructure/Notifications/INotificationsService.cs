using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Core.Infrastructure.Notifications
{
  public interface INotificationsService
  {
    Task SendMessage(MeetingChatMessageDto message, CancellationToken cancellationToken);
    Task SendMessages(ICollection<MeetingChatMessageDto> messages, int userid, CancellationToken cancellationToken);
  }
}
