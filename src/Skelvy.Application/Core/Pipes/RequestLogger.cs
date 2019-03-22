using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Exceptions;

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
      catch (ValidationException exception)
      {
        _logger.LogError(
          "Request Validation Exception: {@Errors}",
          RequestValidationHelper.GetValidationFailures(exception.Errors));
        responseException = exception;
      }
      catch (CustomException exception)
      {
        _logger.LogError("Request {Status}: {Message}", exception.Status, exception.Message);
        responseException = exception;
      }
      catch (Exception exception)
      {
        _logger.LogCritical(exception, "Unexpected Server Exception:");
        responseException = new InternalServerErrorException();
      }

      _timer.Stop();

      if (_timer.ElapsedMilliseconds > 300)
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
