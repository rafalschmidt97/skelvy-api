using FluentValidation;
using Skelvy.Application.Users.Queries.FindUser;

namespace Skelvy.Application.Users.Queries.FindSelf
{
  public class FindSelfQueryValidator : AbstractValidator<FindUserQuery>
  {
    public FindSelfQueryValidator()
    {
      RuleFor(x => x.Id).NotEmpty();
    }
  }
}
