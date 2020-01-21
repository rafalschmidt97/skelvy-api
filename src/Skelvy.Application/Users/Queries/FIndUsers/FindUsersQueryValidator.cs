using FluentValidation;

namespace Skelvy.Application.Users.Queries.FIndUsers
{
  public class FindUsersQueryValidator : AbstractValidator<FindUsersQuery>
  {
    public FindUsersQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.UserName).NotEmpty().MinimumLength(3).MaximumLength(50);
    }
  }
}
