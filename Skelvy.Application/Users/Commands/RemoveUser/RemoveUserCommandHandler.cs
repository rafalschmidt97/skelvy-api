using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Core.Infrastructure.Notifications;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.RemoveUser
{
  public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public RemoveUserCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
      var user = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.Id);
      }

      var meetingUser = await _context.MeetingUsers.FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);

      _context.Users.Remove(user);

      await _context.SaveChangesAsync(cancellationToken);

      if (meetingUser != null)
      {
        await _notifications.BroadcastUserLeftMeeting(meetingUser.MeetingId, cancellationToken);
      }

      return Unit.Value;
    }
  }
}
