using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Users.Queries;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries
{
  public interface IMeetingMapper
  {
    Task<SelfModel> Map(User user, Meeting meeting, IList<Message> messages, MeetingRequest meetingRequest, string language);
    Task<SelfModel> Map(User user, MeetingRequest meetingRequest, string language);
    SelfModel Map(User user);
    Task<MeetingModel> Map(Meeting meeting, IList<Message> messages, MeetingRequest meetingRequest, string language);
    Task<MeetingModel> Map(MeetingRequest meetingRequest, string language);
    Task<MeetingSuggestionsModel> Map(IList<MeetingRequest> requests, IList<Meeting> meetings, string language);
  }
}
