using System.Threading.Tasks;
using Coravel.Invocable;
using MediatR;

namespace Skelvy.WebAPI.Schedulers
{
  public abstract class BaseScheduler : IInvocable
  {
    protected BaseScheduler(IMediator mediator) => Mediator = mediator;
    protected IMediator Mediator { get; }
    public abstract Task Invoke();
  }
}
