using MediatR;

namespace Skelvy.Application.Core.Bus
{
  public interface ICommandData<out TResponse> : IRequest<TResponse>
  {
  }
}
