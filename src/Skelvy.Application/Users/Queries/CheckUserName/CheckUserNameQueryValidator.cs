using FluentValidation;

namespace Skelvy.Application.Users.Queries.CheckUserName
{
  public class CheckUserNameQueryValidator : AbstractValidator<CheckUserNameQuery>
  {
    public CheckUserNameQueryValidator()
    {
      RuleFor(x => x.Name).NotEmpty();
    }
  }
}
