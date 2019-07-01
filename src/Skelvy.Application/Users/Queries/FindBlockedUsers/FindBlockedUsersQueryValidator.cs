using FluentValidation;

namespace Skelvy.Application.Users.Queries.FindBlockedUsers
{
  public class FindBlockedUsersQueryValidator : AbstractValidator<FindBlockedUsersQuery>
  {
    public FindBlockedUsersQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Page).NotEmpty();
    }
  }
}
