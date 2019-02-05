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

      if (context.Exception is CustomException customException)
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
  }
}
