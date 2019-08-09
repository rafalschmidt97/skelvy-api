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
    [HttpGet("self/friends")]
    public async Task<IList<UserDto>> FindPageSelfFriends([FromQuery] FindFriendsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpGet("self/friends/requests")]
    public async Task<IList<FriendRequestDto>> FindAllSelfFriendRequests()
    {
      return await Mediator.Send(new FindFriendRequestsQuery(UserId));
    }

    [HttpPost("self/friends/invite")]
    public async Task InviteSelfFriend(InviteFriendCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPost("self/friends/respond")]
    public async Task RespondSelfFriendRequest(InviteFriendResponseCommand request)
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
