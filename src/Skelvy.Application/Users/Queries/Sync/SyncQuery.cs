using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Users.Queries.Sync
{
  public class SyncQuery : IQuery<SyncModel>
  {
    public SyncQuery(int userId, string language)
    {
      UserId = userId;
      Language = language;
    }

    [JsonConstructor]
    public SyncQuery()
    {
    }

    public int UserId { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}
