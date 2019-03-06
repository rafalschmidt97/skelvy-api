using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Skelvy.WebAPI.Hubs
{
  [Authorize]
  public abstract class BaseHub : Hub
  {
    protected BaseHub(IMediator mediator) => Mediator = mediator;
    protected IMediator Mediator { get; }
    protected int UserId => int.Parse(Context.User.FindFirst(ClaimTypes.Sid).Value);
  }
}
