using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Core.Infrastructure.Notifications;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.AddUserDevice
{
  public class AddUserDeviceCommandHandler : IRequestHandler<AddUserDeviceCommand>
  {
    private readonly SkelvyContext _context;
    private readonly IPushNotificationsService _pushNotifications;

    public AddUserDeviceCommandHandler(SkelvyContext context, IPushNotificationsService pushNotifications)
    {
      _context = context;
      _pushNotifications = pushNotifications;
    }

    public async Task<Unit> Handle(AddUserDeviceCommand request, CancellationToken cancellationToken)
    {
      var user = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var verified = await _pushNotifications.VerifyIds(new[] { request.RegistrationId }, cancellationToken);

      if (verified == null || verified.Success < 1)
      {
        throw new ConflictException("Firebase Registration Id is not valid.");
      }

      var device = await _context.UserDevices
        .FirstOrDefaultAsync(
          x => x.UserId == request.UserId && x.RegistrationId == request.RegistrationId,
          cancellationToken);

      if (device == null)
      {
        var newDevice = new UserDevice
        {
          UserId = request.UserId,
          RegistrationId = request.RegistrationId
        };

        _context.UserDevices.Add(newDevice);
        await _context.SaveChangesAsync(cancellationToken);
      }

      return Unit.Value;
    }
  }
}
