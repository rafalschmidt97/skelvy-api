using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.UpdateUserLanguage
{
  public class UpdateUserLanguageCommand : ICommand
  {
    public UpdateUserLanguageCommand(int userId, string language)
    {
      UserId = userId;
      Language = language;
    }

    public int UserId { get; set; }
    public string Language { get; set; }
  }
}
