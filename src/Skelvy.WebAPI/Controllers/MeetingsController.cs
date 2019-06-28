using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.AddMeetingChatMessage;
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
    public async Task<MeetingModel> Find(int id, [FromQuery] FindMeetingQuery request)
    {
      request.UserId = id;
      return await Mediator.Send(request);
    }

    [HttpGet("self")]
    public async Task<MeetingModel> FindSelf([FromQuery] FindMeetingQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpDelete("{id}")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task Remove(int id)
    {
      await Mediator.Send(new LeaveMeetingCommand(id));
    }

    [HttpDelete("self")]
    public async Task RemoveSelf()
    {
      await Mediator.Send(new LeaveMeetingCommand(UserId));
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

    [HttpPost("{id}/chat")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task AddChatMessage(int id, AddMeetingChatMessageCommand request)
    {
      request.UserId = id;
      await Mediator.Send(request);
    }

    [HttpPost("self/chat")]
    public async Task AddSelfChatMessage(AddMeetingChatMessageCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }
  }
}
