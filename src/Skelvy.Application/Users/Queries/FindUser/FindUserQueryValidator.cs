using FluentValidation;

namespace Skelvy.Application.Users.Queries.FindUser
{
  public class FindUserQueryValidator : AbstractValidator<FindUserQuery>
  {
    public FindUserQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
