using FluentValidation;

namespace Skelvy.Application.Users.Queries.FindSelfUser
{
  public class FindSelfUserQueryValidator : AbstractValidator<FindSelfUserQuery>
  {
    public FindSelfUserQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
