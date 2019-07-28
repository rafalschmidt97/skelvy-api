using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Skelvy.Application.Core.Bus
{
  public abstract class QueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
  {
    public abstract Task<TResponse> Handle(TRequest request);

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
      => Handle(request);
  }
}
