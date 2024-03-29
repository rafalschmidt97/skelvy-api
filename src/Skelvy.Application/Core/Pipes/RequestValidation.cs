using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR.Pipeline;

namespace Skelvy.Application.Core.Pipes
{
  public class RequestValidation<TRequest> : IRequestPreProcessor<TRequest>
  {
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidation(IEnumerable<IValidator<TRequest>> validators)
    {
      _validators = validators;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
      var context = new ValidationContext(request);

      var failures = _validators
        .Select(v => v.Validate(context))
        .SelectMany(result => result.Errors)
        .Where(f => f != null)
        .ToList();

      if (failures.Any())
      {
        throw new ValidationException(failures);
      }

      return Task.CompletedTask;
    }
  }
}
