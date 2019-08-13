using System;
using System.Threading.Tasks;
using MediatR;
using Skelvy.WebAPI.Infrastructure.Notifications;

namespace Skelvy.WebAPI.Hubs
{
  public class UsersHub : BaseHub
  {
    private readonly SignalRBackplane _socket;

    public UsersHub(IMediator mediator, SignalRBackplane socket)
      : base(mediator)
    {
      _socket = socket;
    }

    public override async Task OnConnectedAsync()
    {
      await _socket.ConnectUser(UserId, ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
      await _socket.DisconnectUser(UserId, ConnectionId);
    }
  }
}
