using FluentValidation;

namespace Skelvy.Application.Groups.Queries.FindGroup
{
  public class FindGroupQueryValidator : AbstractValidator<FindGroupQuery>
  {
    public FindGroupQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.GroupId).NotEmpty();
    }
  }
}
