using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.ConnectMeetingRequest;
using Skelvy.Application.Meetings.Commands.CreateMeetingRequest;
using Skelvy.Application.Meetings.Commands.JoinMeeting;
using Skelvy.Application.Meetings.Commands.RemoveMeetingRequest;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeetingSuggestions;
using Skelvy.Application.Users.Commands.DisableUser;
using Skelvy.Application.Users.Commands.RemoveUser;
using Skelvy.Application.Users.Commands.UpdateUserLanguage;
using Skelvy.Application.Users.Commands.UpdateUserProfile;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindSelfUser;
using Skelvy.Application.Users.Queries.FindUser;
using Skelvy.Domain.Enums.Users;
using Skelvy.WebAPI.Filters;

namespace Skelvy.WebAPI.Controllers
{
  public class UsersController : BaseController
  {
    [HttpGet("{id}")]
    public async Task<UserDto> Find(int id)
    {
      return await Mediator.Send(new FindUserQuery(id));
    }

    [HttpGet("self")]
    public async Task<SelfUserDto> FindSelf()
    {
      return await Mediator.Send(new FindSelfUserQuery(UserId));
    }

    [HttpDelete("{id}")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task Remove(int id)
    {
      await Mediator.Send(new RemoveUserCommand(id));
    }

    [HttpPatch("{id}/disabled")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task Disable(int id, DisableUserCommand request)
    {
      request.Id = id;
      await Mediator.Send(request);
    }

    [HttpPatch("{id}/language")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task UpdateLanguage(int id, UpdateUserLanguageCommand request)
    {
      request.UserId = id;
      await Mediator.Send(request);
    }

    [HttpPatch("self/language")]
    public async Task UpdateSelfLanguage(UpdateUserLanguageCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPut("{id}/profile")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task UpdateProfile(int id, UpdateUserProfileCommand request)
    {
      request.UserId = id;
      await Mediator.Send(request);
    }

    [HttpPut("self/profile")]
    public async Task UpdateSelfProfile(UpdateUserProfileCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPost("{id}/request")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task CreateRequest(int id, CreateMeetingRequestCommand request)
    {
      request.UserId = id;
      await Mediator.Send(request);
    }

    [HttpPost("self/request")]
    public async Task CreateRequestSelf(CreateMeetingRequestCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpDelete("{id}/request")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task RemoveRequest(int id)
    {
      await Mediator.Send(new RemoveMeetingRequestCommand(id));
    }

    [HttpDelete("self/request")]
    public async Task RemoveRequestSelf()
    {
      await Mediator.Send(new RemoveMeetingRequestCommand(UserId));
    }

    [HttpGet("{id}/meeting-suggestions")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task<MeetingSuggestionsModel> FindMeetingSuggestions(int id, [FromQuery] FindMeetingSuggestionsQuery request)
    {
      request.UserId = id;
      return await Mediator.Send(request);
    }

    [HttpGet("self/meeting-suggestions")]
    public async Task<MeetingSuggestionsModel> FindSelfMeetingSuggestions([FromQuery] FindMeetingSuggestionsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpPost("{id}/join-meeting/{meetingId}")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task JoinMeeting(int id, int meetingId)
    {
      await Mediator.Send(new JoinMeetingCommand(id, meetingId));
    }

    [HttpPost("self/join-meeting/{meetingId}")]
    public async Task JoinSelfMeeting(int meetingId)
    {
      await Mediator.Send(new JoinMeetingCommand(UserId, meetingId));
    }

    [HttpPost("{id}/connect-request/{requestId}")]
    [AuthorizeRole(RoleTypes.Admin)]
    public async Task ConnectMeetingRequest(int id, int requestId)
    {
      await Mediator.Send(new ConnectMeetingRequestCommand(id, requestId));
    }

    [HttpPost("self/connect-request/{requestId}")]
    public async Task ConnectSelfMeetingRequest(int requestId)
    {
      await Mediator.Send(new ConnectMeetingRequestCommand(UserId, requestId));
    }
  }
}
