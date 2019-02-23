using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Meetings.Commands.DeleteEmptyMeetings;

namespace Skelvy.WebAPI.Schedulers
{
  public class DeleteEmptyMeetingsScheduler : BaseScheduler
  {
    public DeleteEmptyMeetingsScheduler(IMediator mediator)
      : base(mediator)
    {
    }

    public override async Task Invoke()
    {
      await Mediator.Send(new DeleteEmptyMeetingsCommand());
    }
  }
}
