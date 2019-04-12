using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommandHandler : CommandHandler<AddMeetingChatMessageCommand>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationsService _notifications;

    public AddMeetingChatMessageCommandHandler(SkelvyContext context, INotificationsService notifications)
    {
      _context = context;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(AddMeetingChatMessageCommand request)
    {
      var meetingUser =
        await _context.MeetingUsers.FirstOrDefaultAsync(
          x => x.UserId == request.UserId && x.Status == MeetingUserStatusTypes.Joined);

      if (meetingUser == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingUser)}(UserId = {request.UserId}) not found.");
      }

      var message = new MeetingChatMessage
      {
        Message = request.Message.Trim(),
        Date = request.Date,
        UserId = meetingUser.UserId,
        MeetingId = meetingUser.MeetingId,
      };

      _context.MeetingChatMessages.Add(message);

      await _context.SaveChangesAsync();
      await BroadcastMessage(message);
      return Unit.Value;
    }

    private async Task BroadcastMessage(MeetingChatMessage message)
    {
      var meetingUsers = await _context.MeetingUsers
        .Where(x => x.MeetingId == message.MeetingId)
        .ToListAsync();

      var meetingUserIds = meetingUsers.Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserSentMeetingChatMessage(message, meetingUserIds);
    }
  }
}
