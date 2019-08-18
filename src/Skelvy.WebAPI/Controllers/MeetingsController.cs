using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.AddMeeting;
using Skelvy.Application.Meetings.Commands.AddUserToMeeting;
using Skelvy.Application.Meetings.Commands.JoinMeeting;
using Skelvy.Application.Meetings.Commands.LeaveMeeting;
using Skelvy.Application.Meetings.Commands.RemoveMeeting;
using Skelvy.Application.Meetings.Commands.RemoveUserFromMeeting;
using Skelvy.Application.Meetings.Commands.UpdateMeeting;
using Skelvy.Application.Meetings.Commands.UpdateMeetingUserRole;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeetingSuggestions;

namespace Skelvy.WebAPI.Controllers
{
  public class MeetingsController : BaseController
  {
    [HttpPost]
    public async Task Add(AddMeetingCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPut("{id}")]
    public async Task Update(int id, UpdateMeetingCommand request)
    {
      request.MeetingId = id;
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpDelete("{id}")]
    public async Task Remove(int id, RemoveMeetingCommand request)
    {
      request.MeetingId = id;
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPost("{id}/join")]
    public async Task Join(int id)
    {
      await Mediator.Send(new JoinMeetingCommand(UserId, id));
    }

    [HttpPost("{id}/leave")]
    public async Task Leave(int id)
    {
      await Mediator.Send(new LeaveMeetingCommand(id, UserId));
    }

    [HttpPost("{id}/users")]
    public async Task AddUser(int id, AddUserToMeetingCommand request)
    {
      request.UserId = UserId;
      request.MeetingId = id;
      await Mediator.Send(request);
    }

    [HttpDelete("{id}/users/{userId}")]
    public async Task RemoveUser(int id, int userId)
    {
      await Mediator.Send(new RemoveUserFromMeetingCommand(UserId, id, userId));
    }

    [HttpPatch("{id}/users/{userId}/role")]
    public async Task UpdateUserRole(int id, int userId, UpdateMeetingUserRoleCommand request)
    {
      request.UserId = UserId;
      request.MeetingId = id;
      request.UpdatedUserId = userId;
      await Mediator.Send(request);
    }

    [HttpGet("self/suggestions")]
    public async Task<MeetingSuggestionsModel> FindSelfSuggestions([FromQuery] FindMeetingSuggestionsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }
  }
}
