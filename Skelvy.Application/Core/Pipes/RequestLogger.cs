using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Skelvy.Application.Core.Pipes
{
  public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
  {
    private readonly ILogger<TRequest> _logger;

    public RequestLogger(ILogger<TRequest> logger)
    {
      _logger = logger;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
      _logger.LogInformation("Request: {@Request}", request);
      return Task.CompletedTask;
    }
  }
}
