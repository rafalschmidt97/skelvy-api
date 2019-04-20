using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.RemoveMeetingRequest
{
  public class RemoveMeetingRequestCommand : ICommand
  {
    public RemoveMeetingRequestCommand(int userId)
    {
      UserId = userId;
    }

    public int UserId { get; set; }
  }
}
