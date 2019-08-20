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

    public int UserId { get; set; }
    public int InvitationId { get; set; }
    public bool IsAccepted { get; set; }
  }
}
