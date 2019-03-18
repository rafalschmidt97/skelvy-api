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
  public class UsersHub : BaseHub
  {
    private readonly SkelvyContext _context;

    public UsersHub(IMediator mediator, SkelvyContext context)
      : base(mediator)
    {
      _context = context;
    }

    public override async Task OnConnectedAsync()
    {
      var meetingUser = await GetMeetingUser(Context.ConnectionAborted);

      if (meetingUser != null)
      {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(meetingUser.MeetingId), Context.ConnectionAborted);
      }

      await base.OnConnectedAsync();
    }

    public async Task SendMessage(AddMeetingChatMessageCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request, Context.ConnectionAborted);
    }

    public async Task LoadMessages(FindMeetingChatMessagesQuery request)
    {
      request.UserId = UserId;
      await Mediator.Send(request, Context.ConnectionAborted);
    }

    public async Task AddToMeeting()
    {
      var meetingUser = await GetMeetingUser(Context.ConnectionAborted);

      if (meetingUser == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingUser)}(UserId = {UserId}) not found.");
      }

      await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(meetingUser.MeetingId), Context.ConnectionAborted);
    }

    public async Task RemoveFromMeeting(int meetingId)
    {
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(meetingId), Context.ConnectionAborted);
    }

    public static string GetGroupName(int meetingId)
    {
      return $"Meeting:{meetingId}";
    }

    private async Task<MeetingUser> GetMeetingUser(CancellationToken cancellationToken)
    {
      return await _context.MeetingUsers.FirstOrDefaultAsync(x => x.UserId == UserId, cancellationToken);
    }
  }
}
