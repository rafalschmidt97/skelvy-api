namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class EmailMessage
  {
    public EmailMessage(string to, string translatedSubject, string templateName)
    {
      To = to;
      TranslatedSubject = translatedSubject;
      TemplateName = templateName;
    }

    public EmailMessage(string to, string translatedSubject, string templateName, object model)
      : this(to, translatedSubject, templateName)
    {
      Model = model;
    }

    public EmailMessage(string to, string language, string translatedSubject, string templateName)
      : this(to, translatedSubject, templateName)
    {
      Language = language;
    }

    public EmailMessage(string to, string language, string translatedSubject, string templateName, object model)
      : this(to, language, translatedSubject, templateName)
    {
      Model = model;
    }

    public string To { get; }
    public string TranslatedSubject { get; }
    public string TemplateName { get; }
    public string Language { get; }
    public object Model { get; }
  }
}
