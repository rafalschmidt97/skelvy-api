using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Users.Queries.FindSelf
{
  public class FindSelfQuery : IQuery<SelfModel>
  {
    public FindSelfQuery(int userId, string language)
    {
      UserId = userId;
      Language = language;
    }

    public FindSelfQuery() // required for FromQuery attribute
    {
    }

    public int UserId { get; set; }
    public string Language { get; set; } = LanguageTypes.EN;
  }
}
