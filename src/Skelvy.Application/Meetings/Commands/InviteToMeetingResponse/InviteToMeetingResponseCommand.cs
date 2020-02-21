using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.InviteToMeetingResponse
{
  public class InviteToMeetingResponseCommand : ICommand
  {
    public InviteToMeetingResponseCommand(int userId, int invitationId, bool isAccepted)
    {
      UserId = userId;
      InvitationId = invitationId;
      IsAccepted = isAccepted;
    }

    [JsonConstructor]
    public InviteToMeetingResponseCommand()
    {
    }

    public int UserId { get; set; }
    public int InvitationId { get; set; }
    public bool IsAccepted { get; set; }
  }
}
