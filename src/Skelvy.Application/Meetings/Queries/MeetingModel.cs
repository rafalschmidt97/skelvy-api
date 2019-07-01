using System.Collections.Generic;

namespace Skelvy.Application.Meetings.Queries
{
  public class MeetingModel
  {
    public MeetingModel(string status, MeetingDto meeting, IList<MeetingChatMessageDto> messages, MeetingRequestDto request)
    {
      Status = status;
      Meeting = meeting;
      Messages = messages;
      Request = request;
    }

    public MeetingModel(string status, MeetingRequestDto request)
    {
      Status = status;
      Request = request;
    }

    public string Status { get; }
    public MeetingDto Meeting { get; }
    public IList<MeetingChatMessageDto> Messages { get; }
    public MeetingRequestDto Request { get; }
  }
}
