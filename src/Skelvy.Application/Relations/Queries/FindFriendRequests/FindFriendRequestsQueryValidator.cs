using FluentValidation;

namespace Skelvy.Application.Relations.Queries.FindFriendRequests
{
  public class FindFriendRequestsQueryValidator : AbstractValidator<FindFriendRequestsQuery>
  {
    public FindFriendRequestsQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
    }
  }
}
