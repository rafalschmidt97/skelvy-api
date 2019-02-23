using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Meetings.Commands.MatchMeetingRequests;

namespace Skelvy.WebAPI.Schedulers
{
  public class MatchMeetingRequestsScheduler : BaseScheduler
  {
    public MatchMeetingRequestsScheduler(IMediator mediator)
      : base(mediator)
    {
    }

    public override async Task Invoke()
    {
      await Mediator.Send(new MatchMeetingRequestsCommand());
    }
  }
}
