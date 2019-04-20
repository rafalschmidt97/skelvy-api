using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.LeaveMeeting;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeeting;
using Skelvy.Application.Meetings.Queries.FindMeetingChatMessages;
using Skelvy.Domain.Enums.Users;
using Skelvy.WebAPI.Filters;

namespace Skelvy.WebAPI.Controllers
{
  public class MeetingsController : BaseController
  {
    [HttpGet("{id}")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task<MeetingViewModel> Find(int id)
    {
      return await Mediator.Send(new FindMeetingQuery { UserId = id });
    }

    [HttpGet("self")]
    public async Task<MeetingViewModel> FindSelf()
    {
      return await Mediator.Send(new FindMeetingQuery { UserId = UserId });
    }

    [HttpDelete("{id}")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task Remove(int id)
    {
      await Mediator.Send(new LeaveMeetingCommand { UserId = id });
    }

    [HttpDelete("self")]
    public async Task RemoveSelf()
    {
      await Mediator.Send(new LeaveMeetingCommand { UserId = UserId });
    }

    [HttpGet("{id}/chat")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task<IList<MeetingChatMessageDto>> FindChat(int id, [FromQuery] FindMeetingChatMessagesQuery request)
    {
      request.UserId = id;
      return await Mediator.Send(request);
    }

    [HttpGet("self/chat")]
    public async Task<IList<MeetingChatMessageDto>> FindSelfChat([FromQuery] FindMeetingChatMessagesQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }
  }
}
