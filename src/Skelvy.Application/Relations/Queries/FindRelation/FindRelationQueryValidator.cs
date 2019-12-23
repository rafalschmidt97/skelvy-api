using FluentValidation;

namespace Skelvy.Application.Relations.Queries.FindRelation
{
  public class FindRelationQueryValidator : AbstractValidator<FindRelationQuery>
  {
    public FindRelationQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.RelatedUserId).NotEmpty();
    }
  }
}
