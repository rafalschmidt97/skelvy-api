using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Meetings.Commands.DeleteExpiredMeetingsAndMeetingRequests;

namespace Skelvy.WebAPI.Schedulers
{
  public class DeleteExpiredMeetingsAndMeetingRequestsScheduler : BaseScheduler
  {
    public DeleteExpiredMeetingsAndMeetingRequestsScheduler(IMediator mediator)
      : base(mediator)
    {
    }

    public override async Task Invoke()
    {
      await Mediator.Send(new DeleteExpiredMeetingsAndMeetingRequestsCommand());
    }
  }
}
