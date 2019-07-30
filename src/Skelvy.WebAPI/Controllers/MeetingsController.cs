using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.AddMessage;
using Skelvy.Application.Meetings.Commands.LeaveMeeting;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeeting;
using Skelvy.Application.Meetings.Queries.FindMessages;
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
    public async Task<IList<MessageDto>> FindMessages(int id, [FromQuery] FindMessagesQuery request)
    {
      request.UserId = id;
      return await Mediator.Send(request);
    }

    [HttpGet("self/chat")]
    public async Task<IList<MessageDto>> FindSelfMessages([FromQuery] FindMessagesQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpPost("{id}/chat")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task<MessageDto> AddMessage(int id, AddMessageCommand request)
    {
      request.UserId = id;
      return await Mediator.Send(request);
    }

    [HttpPost("self/chat")]
    public async Task<MessageDto> AddSelfMessage(AddMessageCommand request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }
  }
}
