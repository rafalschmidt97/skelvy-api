using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.SendEmailToUser
{
  public class SendEmailToUserCommand : ICommand
  {
    public SendEmailToUserCommand(string to, string subject, string language, string message)
    {
      To = to;
      Subject = subject;
      Language = language;
      Message = message;
    }

    public string To { get; set; }
    public string Subject { get; set; }
    public string Language { get; set; }
    public string Message { get; set; }
  }
}
