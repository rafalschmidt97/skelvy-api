using System;
using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Pipes;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Exceptions;

namespace Skelvy.WebAPI.Filters
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public sealed class CustomExceptionFilter : ExceptionFilterAttribute
  {
    private readonly ILogger<CustomExceptionFilter> _logger;

    public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
    {
      _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
      var status = HttpStatusCode.InternalServerError;
      object message = nameof(HttpStatusCode.InternalServerError);

      if (context.Exception is ValidationException validationException)
      {
        status = HttpStatusCode.BadRequest;
        message = RequestValidationHelper.GetValidationFailures(validationException.Errors);
      }
      else if (context.Exception is CustomException customException)
      {
        status = customException.Status;
        message = customException.Message;
      }
      else if (context.Exception is DomainException domainException)
      {
        status = HttpStatusCode.Conflict;
        message = domainException.Message;
      }
      else
      {
        _logger.LogCritical(context.Exception, "Unexpected Web Layer Exception:");
      }

      context.HttpContext.Response.ContentType = "application/json";
      context.HttpContext.Response.StatusCode = (int)status;
      context.Result = new JsonResult(new { status, message });
    }
  }
}
