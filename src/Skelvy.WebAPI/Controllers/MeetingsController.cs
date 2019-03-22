using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.CreateMeetingRequest;
using Skelvy.Application.Meetings.Commands.LeaveMeeting;
using Skelvy.Application.Meetings.Commands.RemoveMeetingRequest;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeeting;
using Skelvy.Application.Meetings.Queries.FindMeetingChatMessages;

namespace Skelvy.WebAPI.Controllers
{
  public class MeetingsController : BaseController
  {
    [HttpGet("self")]
    public async Task<MeetingViewModel> FindSelf()
    {
      return await Mediator.Send(new FindMeetingQuery { UserId = UserId }, HttpContext.RequestAborted);
    }

    [HttpDelete("self")]
    public async Task RemoveSelf()
    {
      await Mediator.Send(new LeaveMeetingCommand { UserId = UserId }, HttpContext.RequestAborted);
    }

    [HttpPost("requests/self")]
    public async Task CreateRequestSelf(CreateMeetingRequestCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request, HttpContext.RequestAborted);
    }

    [HttpDelete("requests/self")]
    public async Task RemoveRequestSelf()
    {
      await Mediator.Send(new RemoveMeetingRequestCommand { UserId = UserId }, HttpContext.RequestAborted);
    }

    [HttpGet("self/chat")]
    public async Task<ICollection<MeetingChatMessageDto>> FindSelfChat([FromQuery] FindMeetingChatMessagesQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request, HttpContext.RequestAborted);
    }
  }
}
