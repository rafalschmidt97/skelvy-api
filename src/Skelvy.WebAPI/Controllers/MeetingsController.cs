using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.AddMeeting;
using Skelvy.Application.Meetings.Commands.AddUserToMeeting;
using Skelvy.Application.Meetings.Commands.JoinMeeting;
using Skelvy.Application.Meetings.Commands.LeaveMeeting;
using Skelvy.Application.Meetings.Commands.RemoveMeeting;
using Skelvy.Application.Meetings.Commands.RemoveUserFromMeeting;
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

    [HttpPut("{id}")]
    public async Task UpdateSelfMeeting(int id, UpdateMeetingCommand request)
    {
      request.MeetingId = id;
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpDelete("{id}")]
    public async Task RemoveSelfMeeting(int id, RemoveMeetingCommand request)
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

    [HttpPost("{id}/users")]
    public async Task AddSelfUserToMeeting(int id, AddUserToMeetingCommand request)
    {
      request.UserId = UserId;
      request.MeetingId = id;
      await Mediator.Send(request);
    }

    [HttpDelete("{id}/users/{userId}")]
    public async Task RemoveSelfUserFromMeeting(int id, int userId)
    {
      await Mediator.Send(new RemoveUserFromMeetingCommand(UserId, id, userId));
    }

    [HttpGet("self/suggestions")]
    public async Task<MeetingSuggestionsModel> FindSelfMeetingSuggestions([FromQuery] FindMeetingSuggestionsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }
  }
}
