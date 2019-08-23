using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries
{
  public interface IMeetingMapper
  {
    Task<MeetingDto> Map(Meeting meeting, string language);
    Task<IList<MeetingDto>> Map(IList<Meeting> meetings, string language);
    Task<MeetingRequestDto> Map(MeetingRequest meetingRequest, string language);
    Task<IList<MeetingRequestDto>> Map(IList<MeetingRequest> meetingRequests, string language);
    Task<MeetingInvitationDto> Map(MeetingInvitation meetingInvitation, string language);
    Task<IList<MeetingInvitationDto>> Map(IList<MeetingInvitation> meetingInvitations, string language);
    Task<MeetingSuggestionsModel> Map(IList<MeetingRequest> requests, IList<Meeting> meetings, string language);
  }
}
