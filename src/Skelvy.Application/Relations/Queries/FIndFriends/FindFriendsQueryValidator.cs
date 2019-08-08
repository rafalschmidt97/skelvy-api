using FluentValidation;

namespace Skelvy.Application.Relations.Queries.FIndFriends
{
  public class FindFriendsQueryValidator : AbstractValidator<FindFriendsQuery>
  {
    public FindFriendsQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Page).NotEmpty();
    }
  }
}
