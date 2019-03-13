using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingRequests;

namespace Skelvy.WebAPI.Schedulers
{
  public class RemoveExpiredMeetingRequestsScheduler : BaseScheduler
  {
    public RemoveExpiredMeetingRequestsScheduler(IMediator mediator)
      : base(mediator)
    {
    }

    public override async Task Invoke()
    {
      await Mediator.Send(new RemoveExpiredMeetingRequestsCommand());
    }
  }
}
