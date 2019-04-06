using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Users.Commands.RemoveUsers;

namespace Skelvy.WebAPI.Schedulers
{
  public class RemoveUsersScheduler : BaseScheduler
  {
    public RemoveUsersScheduler(IMediator mediator)
      : base(mediator)
    {
    }

    public override async Task Invoke()
    {
      await Mediator.Send(new RemoveUsersCommand());
    }
  }
}
