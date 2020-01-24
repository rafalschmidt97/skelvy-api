using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.SendEmailToUsers
{
  public class SendEmailToUsersCommand : ICommand
  {
    public SendEmailToUsersCommand(int minId, int maxId, string subject, string language, string message)
    {
      MinId = minId;
      MaxId = maxId;
      Subject = subject;
      Language = language;
      Message = message;
    }

    public int MinId { get; set; }
    public int MaxId { get; set; }
    public string Subject { get; set; }
    public string Language { get; set; }
    public string Message { get; set; }
  }
}
