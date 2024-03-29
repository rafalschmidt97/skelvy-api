using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Application.Core.Pipes
{
  public class RequestLogger<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    private readonly ILogger<TRequest> _logger;
    private readonly Stopwatch _timer;

    public RequestLogger(ILogger<TRequest> logger)
    {
      _logger = logger;
      _timer = new Stopwatch();
    }

    public async Task<TResponse> Handle(
      TRequest request,
      CancellationToken cancellationToken,
      RequestHandlerDelegate<TResponse> next)
    {
      _logger.LogInformation("Request: {@Request}", request);

      _timer.Start();

      var response = default(TResponse);
      Exception responseException = null;

      try
      {
        response = await next();
      }
      catch (CustomException exception)
      {
        _logger.LogError("Request {Status}: {Message}", exception.Status, exception.Message);
        responseException = exception;
      }
      catch (DomainException exception)
      {
        _logger.LogError("Domain Exception: {Message}", exception.Message);
        responseException = exception;
      }
      catch (DbUpdateConcurrencyException)
      {
        _logger.LogError("Request Concurrency Exception: {@Request}", request);
        responseException =
          new ConflictException("Data have been modified since entities were loaded.");
      }
      catch (Exception exception)
      {
        _logger.LogCritical(exception, "Unexpected Server Exception:");
        responseException = new InternalServerErrorException();
      }

      _timer.Stop();

      if (_timer.ElapsedMilliseconds > 2000)
      {
        _logger.LogWarning(
          "Request Performance Issue: {@Request} ({ElapsedMilliseconds} milliseconds)",
          request,
          _timer.ElapsedMilliseconds);
      }

      if (responseException != null)
      {
        throw responseException;
      }

      return response;
    }
  }
}
