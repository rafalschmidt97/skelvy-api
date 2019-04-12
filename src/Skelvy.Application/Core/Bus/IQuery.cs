using MediatR;

namespace Skelvy.Application.Core.Bus
{
  public interface IQuery<out TResponse> : IRequest<TResponse>
  {
  }
}
