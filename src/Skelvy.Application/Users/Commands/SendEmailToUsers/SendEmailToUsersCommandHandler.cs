using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Notifications.Infrastructure;
using Skelvy.Application.Users.Infrastructure.Notifications;
using Skelvy.Application.Users.Infrastructure.Repositories;

namespace Skelvy.Application.Users.Commands.SendEmailToUsers
{
  public class SendEmailToUsersCommandHandler : CommandHandler<SendEmailToUsersCommand>
  {
    private readonly IEmailNotificationsService _emailService;
    private readonly IUsersRepository _usersRepository;

    public SendEmailToUsersCommandHandler(IEmailNotificationsService emailService, IUsersRepository usersRepository)
    {
      _emailService = emailService;
      _usersRepository = usersRepository;
    }

    public override async Task<Unit> Handle(SendEmailToUsersCommand request)
    {
      var users = await _usersRepository.FindAllBetweenId(request.MinId, request.MaxId);

      foreach (var user in users.Where(x => x.Email != null))
      {
        await _emailService.BroadcastCustomMessage(
          new CustomEmailNotification(user.Email, request.Subject, request.Language, request.Message));

        await Task.Delay(1000);
      }

      return Unit.Value;
    }
  }
}
