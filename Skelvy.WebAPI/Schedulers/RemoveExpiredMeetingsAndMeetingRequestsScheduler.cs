using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingsAndMeetingRequests;

namespace Skelvy.WebAPI.Schedulers
{
  public class RemoveExpiredMeetingsAndMeetingRequestsScheduler : BaseScheduler
  {
    public RemoveExpiredMeetingsAndMeetingRequestsScheduler(IMediator mediator)
      : base(mediator)
    {
    }

    public override async Task Invoke()
    {
      await Mediator.Send(new RemoveExpiredMeetingsAndMeetingRequestsCommand());
    }
  }
}
