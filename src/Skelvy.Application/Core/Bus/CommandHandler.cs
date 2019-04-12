using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Skelvy.Application.Core.Bus
{
  public abstract class CommandHandler<TRequest> : IRequestHandler<TRequest>
    where TRequest : IRequest<Unit>
  {
    public abstract Task<Unit> Handle(TRequest request);

    public Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
      => Handle(request);
  }
}
