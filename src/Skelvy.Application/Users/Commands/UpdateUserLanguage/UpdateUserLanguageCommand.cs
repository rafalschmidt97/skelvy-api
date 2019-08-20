using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Users.Commands.UpdateUserLanguage
{
  public class UpdateUserLanguageCommand : ICommand
  {
    public UpdateUserLanguageCommand(int userId, string language)
    {
      UserId = userId;
      Language = language;
    }

    public UpdateUserLanguageCommand() // required for FromBody attribute to allow default values
    {
    }

    public int UserId { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}
