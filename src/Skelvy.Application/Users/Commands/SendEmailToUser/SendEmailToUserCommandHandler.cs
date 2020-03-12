using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications.Infrastructure;
using Skelvy.Application.Users.Infrastructure.Notifications;

namespace Skelvy.Application.Users.Commands.SendEmailToUser
{
  public class SendEmailToUserCommandHandler : CommandHandler<SendEmailToUserCommand>
  {
    private readonly IEmailNotificationsService _emailService;

    public SendEmailToUserCommandHandler(IEmailNotificationsService emailService)
    {
      _emailService = emailService;
    }

    public override async Task<Unit> Handle(SendEmailToUserCommand request)
    {
      foreach (var email in request.To)
      {
        await _emailService.BroadcastCustomMessage(
          new CustomEmailNotification(email, request.Subject, request.Language, request.Message));

        await Task.Delay(1000);
      }

      return Unit.Value;
    }
  }
}
