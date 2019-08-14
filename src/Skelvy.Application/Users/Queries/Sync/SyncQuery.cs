using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Users.Queries.Sync
{
  public class SyncQuery : IQuery<SyncModel>
  {
    public SyncQuery(int userId, string language)
    {
      UserId = userId;
      Language = language;
    }

    public SyncQuery() // required for FromQuery attribute
    {
    }

    public int UserId { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}
