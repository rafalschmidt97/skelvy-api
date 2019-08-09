using FluentValidation;

namespace Skelvy.Application.Messages.Queries.FindMessages
{
  public class FindMessagesQueryValidator : AbstractValidator<FindMessagesQuery>
  {
    public FindMessagesQueryValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.GroupId).NotEmpty();
      RuleFor(x => x.BeforeDate).NotEmpty();
    }
  }
}
