using MediatR;

namespace Skelvy.Application.Core.Bus
{
  public interface ICommand : IRequest<Unit>
  {
  }
}
