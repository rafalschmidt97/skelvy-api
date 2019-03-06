using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Application.Meetings.Commands.AddMeetingChatMessage;
using Skelvy.Application.Meetings.Queries.FindMeetingChatMessages;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.WebAPI.Hubs
{
  public class MeetingHub : BaseHub
  {
    private readonly SkelvyContext _context;

    public MeetingHub(IMediator mediator, SkelvyContext context)
      : base(mediator)
    {
      _context = context;
    }

    public override async Task OnConnectedAsync()
    {
      var meetingUser = await GetMeetingUser(Context.ConnectionAborted);
      await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(meetingUser.MeetingId), Context.ConnectionAborted);
      await base.OnConnectedAsync();
    }

    public async Task SendMessage(string message)
    {
      var request = new AddMeetingChatMessageCommand { Message = message, UserId = UserId };
      await Mediator.Send(request, Context.ConnectionAborted);
    }

    public async Task LoadMessages(FindMeetingChatMessagesQuery request)
    {
      request.UserId = UserId;
      await Mediator.Send(request, Context.ConnectionAborted);
    }

    public static string GetGroupName(int meetingId)
    {
      return $"Meeting:{meetingId}";
    }

    private async Task<MeetingUser> GetMeetingUser(CancellationToken cancellationToken)
    {
      var meetingUser = await _context.MeetingUsers.FirstOrDefaultAsync(x => x.UserId == UserId, cancellationToken);

      if (meetingUser == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingUser)}(UserId = {UserId}) not found.");
      }

      return meetingUser;
    }
  }
}
