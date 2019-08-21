using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Relations.Commands.AddBlocked;
using Skelvy.Application.Relations.Commands.InviteFriend;
using Skelvy.Application.Relations.Commands.InviteFriendResponse;
using Skelvy.Application.Relations.Commands.RemoveBlocked;
using Skelvy.Application.Relations.Commands.RemoveFriend;
using Skelvy.Application.Relations.Queries;
using Skelvy.Application.Relations.Queries.FindBlocked;
using Skelvy.Application.Relations.Queries.FindFriendInvitations;
using Skelvy.Application.Relations.Queries.FindFriends;
using Skelvy.Application.Users.Queries;

namespace Skelvy.WebAPI.Controllers
{
  public class RelationsController : BaseController
  {
    [HttpGet("self/friends")]
    public async Task<IList<UserDto>> FindPageSelfFriends([FromQuery] FindFriendsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpGet("self/friends/invitations")]
    public async Task<IList<FriendInvitationsDto>> FindAllSelfFriendInvitations()
    {
      return await Mediator.Send(new FindFriendInvitationsQuery(UserId));
    }

    [HttpPost("self/friends/invite")]
    public async Task InviteSelfFriend(InviteFriendCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPost("self/friends/respond")]
    public async Task RespondSelfFriendInvitation(InviteFriendResponseCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpDelete("self/friends/{userId}")]
    public async Task RemoveSelfFriend(int userId)
    {
      await Mediator.Send(new RemoveFriendCommand(UserId, userId));
    }

    [HttpGet("self/blocked")]
    public async Task<IList<UserDto>> FindPageSelfBlocked([FromQuery] FindBlockedQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpPost("self/blocked")]
    public async Task AddSelfBlocked(AddBlockedCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpDelete("self/blocked/{userId}")]
    public async Task RemoveSelfBlocked(int userId)
    {
      await Mediator.Send(new RemoveBlockedCommand(UserId, userId));
    }
  }
}
