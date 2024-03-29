using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Queries.FindMeetingSuggestions
{
  public class FindMeetingSuggestionsQuery : IQuery<MeetingSuggestionsModel>
  {
    public FindMeetingSuggestionsQuery(int userId, double latitude, double longitude, string language)
    {
      UserId = userId;
      Latitude = latitude;
      Longitude = longitude;
      Language = language;
    }

    [JsonConstructor]
    public FindMeetingSuggestionsQuery()
    {
    }

    public int UserId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}
