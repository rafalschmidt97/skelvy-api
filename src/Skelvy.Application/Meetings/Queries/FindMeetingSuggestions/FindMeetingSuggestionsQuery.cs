using Skelvy.Application.Core.Bus;

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

    public FindMeetingSuggestionsQuery() // required for FromQuery attribute
    {
    }

    public int UserId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Language { get; set; }
  }
}
