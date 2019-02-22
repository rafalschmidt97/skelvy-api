using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.CreateMeetingRequest;
using Skelvy.Application.Meetings.Commands.LeaveMeeting;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Meetings.Queries.GetMeeting;

namespace Skelvy.WebAPI.Controllers
{
  public class MeetingsController : BaseController
  {
    [HttpGet("self")]
    public async Task<MeetingViewModel> GetSelf()
    {
      return await Mediator.Send(new GetMeetingQuery { UserId = UserId });
    }

    [HttpDelete("self")]
    public async Task DeleteSelf()
    {
      await Mediator.Send(new LeaveMeetingCommand { UserId = UserId });
    }

    [HttpPost("requests/self")]
    public async Task CreateRequestSelf(CreateMeetingRequestCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }
  }
}
