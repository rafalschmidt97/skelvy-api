using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Domain.Entities;

namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class EmailNotificationsService : IEmailNotificationsService
  {
    private readonly IFluentEmail _email;

    public EmailNotificationsService(IFluentEmail email)
    {
      _email = email;
    }

    public async Task BroadcastUserCreated(User user, CancellationToken cancellationToken)
    {
      var message = new EmailMessage
      {
        To = user.Email,
        Subject = "Account has been created",
        Body = "Body created"
      };

      await SendEmail(message, cancellationToken);
    }

    public async Task BroadcastUserDeleted(User user, CancellationToken cancellationToken)
    {
      var message = new EmailMessage
      {
        To = user.Email,
        Subject = "Account has been deleted",
        Body = "Body deleted"
      };

      await SendEmail(message, cancellationToken);
    }

    private async Task SendEmail(EmailMessage message, CancellationToken cancellationToken)
    {
      await _email
        .To(message.To)
        .Subject(message.Subject)
        .Body(message.Body)
        .SendAsync(cancellationToken);
    }
  }

  public class EmailMessage
  {
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
  }
}
