using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Skelvy.Application.Core.Pipes;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Exceptions;

namespace Skelvy.WebAPI.Extensions
{
  public static class ExceptionExtension
  {
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
  }

  public class CustomExceptionHandlerMiddleware : IMiddleware
  {
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

    public CustomExceptionHandlerMiddleware(ILogger<CustomExceptionHandlerMiddleware> logger)
    {
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      try
      {
        await next(context);
      }
      catch (Exception exception)
      {
        await HandleExceptionAsync(context, exception);
      }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      var status = HttpStatusCode.InternalServerError;
      object message = nameof(HttpStatusCode.InternalServerError);

      if (exception is ValidationException validationException)
      {
        status = HttpStatusCode.BadRequest;
        message = RequestValidationHelper.GetValidationFailures(validationException.Errors);
        _logger.LogError(
          "Request Validation Exception: {@Errors}",
          RequestValidationHelper.GetValidationFailures(validationException.Errors));
      }
      else if (exception is CustomException customException)
      {
        status = customException.Status;
        message = customException.Message;
      }
      else if (exception is DomainException domainException)
      {
        status = HttpStatusCode.Conflict;
        message = domainException.Message;
      }
      else
      {
        _logger.LogCritical(exception, "Unexpected Web Layer Exception:");
      }

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)status;

      var result = JsonConvert.SerializeObject(new { status, message });
      return context.Response.WriteAsync(result);
    }
  }
}
