using MediatR;

namespace Skelvy.Application.Users.Commands.UpdateUserLanguage
{
  public class UpdateUserLanguageCommand : IRequest
  {
    public int UserId { get; set; }
    public string Language { get; set; }
  }
}
