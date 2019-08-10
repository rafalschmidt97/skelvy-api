using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.ConnectMeetingRequest;
using Skelvy.Application.Meetings.Commands.JoinMeeting;
using Skelvy.Application.Meetings.Commands.LeaveMeeting;
using Skelvy.Application.Meetings.Commands.RemoveMeetingRequest;
using Skelvy.Application.Meetings.Commands.SearchMeeting;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeeting;
using Skelvy.Application.Meetings.Queries.FindMeetingSuggestions;

namespace Skelvy.WebAPI.Controllers
{
  public class MeetingsController : BaseController
  {
    [HttpGet("self")]
    public async Task<MeetingModel> FindAllSelf([FromQuery] FindMeetingQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpPost("self/leave")]
    public async Task LeaveSelf()
    {
      await Mediator.Send(new LeaveMeetingCommand(UserId));
    }

    [HttpPost("self/requests")]
    public async Task CreateSelfRequest(SearchMeetingCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpDelete("self/requests")]
    public async Task RemoveSelfRequest()
    {
      await Mediator.Send(new RemoveMeetingRequestCommand(UserId));
    }

    [HttpGet("self/suggestions")]
    public async Task<MeetingSuggestionsModel> FindSelfMeetingSuggestions([FromQuery] FindMeetingSuggestionsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpPost("{id}/join")]
    public async Task JoinMeeting(int id)
    {
      await Mediator.Send(new JoinMeetingCommand(UserId, id));
    }

    [HttpPost("self/requests/{id}/connect")]
    public async Task ConnectSelfMeetingRequest(int id)
    {
      await Mediator.Send(new ConnectMeetingRequestCommand(UserId, id));
    }
  }
}
