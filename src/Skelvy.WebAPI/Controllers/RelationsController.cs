using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Relations.Commands.InviteFriend;
using Skelvy.Application.Relations.Commands.InviteFriendResponse;
using Skelvy.Application.Relations.Commands.RemoveFriend;
using Skelvy.Application.Relations.Queries;
using Skelvy.Application.Relations.Queries.FindFriendRequests;
using Skelvy.Application.Relations.Queries.FIndFriends;
using Skelvy.Application.Users.Queries;

namespace Skelvy.WebAPI.Controllers
{
  public class RelationsController : BaseController
  {
    [HttpGet("relations/self/friends")]
    public async Task<IList<UserDto>> FindAllSelfFriends([FromQuery] FindFriendsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpGet("relations/self/friends/pending")]
    public async Task<IList<FriendRequestDto>> FindAllSelfPendingFriends()
    {
      return await Mediator.Send(new FindFriendRequestsQuery(UserId));
    }

    [HttpPost("relations/self/friends")]
    public async Task InviteToFriends(InviteFriendCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpGet("relations/self/friends/requests")]
    public async Task<IList<FriendRequestDto>> FindAllSelfFriendRequests()
    {
      return await Mediator.Send(new FindFriendRequestsQuery(UserId));
    }

    [HttpPost("relations/self/friends/respond")]
    public async Task RespondToSelfRequest(InviteFriendResponseCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpDelete("relations/self/friends/{userId}")]
    public async Task RemoveSelfFriend(int userId)
    {
      await Mediator.Send(new RemoveFriendCommand(UserId, userId));
    }
  }
}
