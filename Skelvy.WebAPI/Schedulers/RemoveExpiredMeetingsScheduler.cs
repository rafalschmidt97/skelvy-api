using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Meetings.Commands.RemoveExpiredMeetings;

namespace Skelvy.WebAPI.Schedulers
{
  public class RemoveExpiredMeetingsScheduler : BaseScheduler
  {
    public RemoveExpiredMeetingsScheduler(IMediator mediator)
      : base(mediator)
    {
    }

    public override async Task Invoke()
    {
      await Mediator.Send(new RemoveExpiredMeetingsCommand());
    }
  }
}
