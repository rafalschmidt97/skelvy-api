using FluentValidation;

namespace Skelvy.Application.Relations.Queries.FindBlocked
{
  public class FindBlockedQueryValidator : AbstractValidator<FindBlockedQuery>
  {
    public FindBlockedQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Page).NotEmpty();
    }
  }
}
