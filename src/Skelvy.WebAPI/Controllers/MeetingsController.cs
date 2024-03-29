using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.AddMeeting;
using Skelvy.Application.Meetings.Commands.InviteToMeeting;
using Skelvy.Application.Meetings.Commands.InviteToMeetingResponse;
using Skelvy.Application.Meetings.Commands.JoinMeeting;
using Skelvy.Application.Meetings.Commands.LeaveMeeting;
using Skelvy.Application.Meetings.Commands.RemoveMeeting;
using Skelvy.Application.Meetings.Commands.UpdateMeeting;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeeting;
using Skelvy.Application.Meetings.Queries.FindMeetingInvitations;
using Skelvy.Application.Meetings.Queries.FindMeetings;
using Skelvy.Application.Meetings.Queries.FindMeetingSuggestions;
using Skelvy.Application.Meetings.Queries.FindUsersToInviteToMeeting;
using Skelvy.Application.Users.Queries;

namespace Skelvy.WebAPI.Controllers
{
  public class MeetingsController : BaseController
  {
    [HttpGet("self")]
    public async Task<MeetingModel> FindSelf([FromQuery] FindMeetingsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpGet("{id}")]
    public async Task<MeetingDto> Find(int id, [FromQuery] FindMeetingQuery request)
    {
      request.MeetingId = id;
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

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
    public async Task Remove(int id)
    {
      await Mediator.Send(new RemoveMeetingCommand(id, UserId));
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

    [HttpGet("self/suggestions")]
    public async Task<MeetingSuggestionsModel> FindSelfSuggestions([FromQuery] FindMeetingSuggestionsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpGet("{meetingId}/invitations")]
    public async Task<IList<UserDto>> FindUsersToInvite(int meetingId, [FromQuery] FindUsersToInviteToMeetingQuery request)
    {
      request.MeetingId = meetingId;
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpGet("self/invitations")]
    public async Task<IList<SelfMeetingInvitationDto>> FindAllSelfInvitations([FromQuery] FindMeetingInvitationsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpPost("self/invitations")]
    public async Task Invite(InviteToMeetingCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPost("self/invitations/{invitationId}/respond")]
    public async Task RespondInvite(int invitationId, InviteToMeetingResponseCommand request)
    {
      request.UserId = UserId;
      request.InvitationId = invitationId;
      await Mediator.Send(request);
    }
  }
}
