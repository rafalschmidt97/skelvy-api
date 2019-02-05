using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Skelvy.Application.Core.Pipes
{
  public class RequestPerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    private readonly ILogger<TRequest> _logger;
    private readonly Stopwatch _timer;

    public RequestPerformanceBehavior(ILogger<TRequest> logger)
    {
      _logger = logger;
      _timer = new Stopwatch();
    }

    public async Task<TResponse> Handle(
      TRequest request,
      CancellationToken cancellationToken,
      RequestHandlerDelegate<TResponse> next)
    {
      _timer.Start();

      var response = await next();

      _timer.Stop();

      if (_timer.ElapsedMilliseconds > 500)
      {
        _logger.LogWarning(
          "Long Running Request: {@Request} ({ElapsedMilliseconds} milliseconds)",
          request,
          _timer.ElapsedMilliseconds);
      }

      return response;
    }
  }
}
