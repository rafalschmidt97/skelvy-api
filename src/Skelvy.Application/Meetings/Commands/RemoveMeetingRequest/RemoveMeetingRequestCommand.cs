using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.RemoveMeetingRequest
{
  public class RemoveMeetingRequestCommand : ICommand
  {
    public RemoveMeetingRequestCommand(int requestId, int userId)
    {
      RequestId = requestId;
      UserId = userId;
    }

    public int RequestId { get; set; }
    public int UserId { get; set; }
  }
}
