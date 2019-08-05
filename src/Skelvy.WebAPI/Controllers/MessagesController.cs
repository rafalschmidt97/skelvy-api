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
  public class MessagesController : BaseController
  {
    [HttpGet("{id}")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task<IList<MessageDto>> FindMessages(int id, [FromQuery] FindMessagesQuery request)
    {
      request.UserId = id;
      return await Mediator.Send(request);
    }

    [HttpGet("self")]
    public async Task<IList<MessageDto>> FindSelfMessages([FromQuery] FindMessagesQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpPost("{id}")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task<IList<MessageDto>> AddMessage(int id, AddMessageCommand request)
    {
      request.UserId = id;
      return await Mediator.Send(request);
    }

    [HttpPost("self")]
    public async Task<IList<MessageDto>> AddSelfMessage(AddMessageCommand request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }
  }
}
