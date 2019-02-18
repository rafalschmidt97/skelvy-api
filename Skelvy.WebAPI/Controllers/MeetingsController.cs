using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Meetings.Commands.CreateMeetingRequest;

namespace Skelvy.WebAPI.Controllers
{
  public class MeetingsController : BaseController
  {
    [HttpPost("requests/self")]
    public async Task CreateRequestSelf(CreateMeetingRequestCommand request)
    {
      request.UserId = UserId;
      await Mediator.Send(request);
    }
  }
}
