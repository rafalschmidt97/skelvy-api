using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.AddMeeting;
using Skelvy.Application.Meetings.Commands.AddMeetingRequest;
using Skelvy.Application.Meetings.Commands.ConnectMeetingRequest;
using Skelvy.Application.Meetings.Commands.JoinMeeting;
using Skelvy.Application.Meetings.Commands.LeaveMeeting;
using Skelvy.Application.Meetings.Commands.RemoveMeetingRequest;
using Skelvy.Application.Meetings.Commands.UpdateMeeting;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeetingSuggestions;

namespace Skelvy.WebAPI.Controllers
{
  public class MeetingsController : BaseController
  {
    [HttpPost]
    public async Task AddSelfMeeting(AddMeetingCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPost("{id}")]
    public async Task UpdateSelfMeeting(int id, UpdateMeetingCommand request)
    {
      request.MeetingId = id;
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPost("{id}/join")]
    public async Task JoinSelfMeeting(int id)
    {
      await Mediator.Send(new JoinMeetingCommand(UserId, id));
    }

    [HttpPost("{id}/leave")]
    public async Task LeaveSelf(int id)
    {
      await Mediator.Send(new LeaveMeetingCommand(id, UserId));
    }

    [HttpPost("self/requests")]
    public async Task SearchSelfRequest(AddMeetingRequestCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPost("self/requests/{id}/connect")]
    public async Task ConnectSelfMeetingRequest(int id, ConnectMeetingRequestCommand request)
    {
      request.UserId = UserId;
      request.MeetingRequestId = id;
      await Mediator.Send(request);
    }

    [HttpDelete("self/requests/{id}")]
    public async Task RemoveSelfRequest(int id)
    {
      await Mediator.Send(new RemoveMeetingRequestCommand(id, UserId));
    }

    [HttpGet("self/suggestions")]
    public async Task<MeetingSuggestionsModel> FindSelfMeetingSuggestions([FromQuery] FindMeetingSuggestionsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }
  }
}
