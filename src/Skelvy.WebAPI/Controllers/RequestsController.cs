using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.AddMeetingRequest;
using Skelvy.Application.Meetings.Commands.ConnectMeetingRequest;
using Skelvy.Application.Meetings.Commands.RemoveMeetingRequest;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.FindMeetingRequests;

namespace Skelvy.WebAPI.Controllers
{
  public class RequestsController : BaseController
  {
    [HttpGet("self")]
    public async Task<IList<MeetingRequestDto>> FindSelf([FromQuery] FindMeetingRequestsQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpPost]
    public async Task Add(AddMeetingRequestCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }

    [HttpPost("{id}/connect")]
    public async Task Connect(int id, ConnectMeetingRequestCommand request)
    {
      request.UserId = UserId;
      request.MeetingRequestId = id;
      await Mediator.Send(request);
    }

    [HttpDelete("{id}")]
    public async Task Remove(int id)
    {
      await Mediator.Send(new RemoveMeetingRequestCommand(id, UserId));
    }
  }
}
