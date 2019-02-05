using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Exceptions;

namespace Skelvy.WebAPI.Filters
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class CustomExceptionFilter : ExceptionFilterAttribute
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
        message = GetValidationFailures(validationException.Errors);
      }
      else if (context.Exception is CustomException customException)
      {
        status = customException.Status;
        message = customException.Message;
      }
      else
      {
        _logger.LogError(context.Exception, message.ToString());
      }

      context.HttpContext.Response.ContentType = "application/json";
      context.HttpContext.Response.StatusCode = (int)status;
      context.Result = new JsonResult(new { status, message });
    }

    private static Dictionary<string, string[]> GetValidationFailures(IEnumerable<ValidationFailure> failures)
    {
      var validationFailures = failures.ToList();
      var propertyNames = validationFailures
        .Select(e => e.PropertyName)
        .Distinct();

      var message = new Dictionary<string, string[]>();

      foreach (var propertyName in propertyNames)
      {
        var propertyFailures = validationFailures
          .Where(e => e.PropertyName == propertyName)
          .Select(e => e.ErrorMessage)
          .ToArray();

        message.Add(propertyName, propertyFailures);
      }

      return message;
    }
  }
}
