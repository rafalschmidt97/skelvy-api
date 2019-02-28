using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Meetings.Commands.RemoveEmptyMeetings;

namespace Skelvy.WebAPI.Schedulers
{
  public class RemoveEmptyMeetingsScheduler : BaseScheduler
  {
    public RemoveEmptyMeetingsScheduler(IMediator mediator)
      : base(mediator)
    {
    }

    public override async Task Invoke()
    {
      await Mediator.Send(new RemoveEmptyMeetingsCommand());
    }
  }
}
