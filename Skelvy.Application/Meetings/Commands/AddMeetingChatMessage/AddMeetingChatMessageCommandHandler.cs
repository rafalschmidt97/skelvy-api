using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Core.Infrastructure.Notifications;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommandHandler : IRequestHandler<AddMeetingChatMessageCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public AddMeetingChatMessageCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public async Task<Unit> Handle(AddMeetingChatMessageCommand request, CancellationToken cancellationToken)
    {
      var meetingUser =
        await _context.MeetingUsers.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

      if (meetingUser == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingUser)}(UserId = {request.UserId}) not found.");
      }

      var message = new MeetingChatMessage
      {
        Message = request.Message,
        Date = DateTime.UtcNow,
        UserId = meetingUser.UserId,
        MeetingId = meetingUser.MeetingId
      };

      _context.MeetingChatMessages.Add(message);

      await _context.SaveChangesAsync(cancellationToken);
      await _notifications.SendMessage(message, cancellationToken);
      return Unit.Value;
    }
  }
}
