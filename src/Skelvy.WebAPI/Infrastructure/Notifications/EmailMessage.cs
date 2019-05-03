namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class EmailMessage
  {
    public EmailMessage(string to, EmailMessageSubject subject, string templateName)
    {
      To = to;
      Subject = subject;
      TemplateName = templateName;
    }

    public EmailMessage(string to, EmailMessageSubject subject, string templateName, object model)
      : this(to, subject, templateName)
    {
      Model = model;
    }

    public EmailMessage(string to, string language, EmailMessageSubject subject, string templateName)
      : this(to, subject, templateName)
    {
      Language = language;
    }

    public EmailMessage(string to, string language, EmailMessageSubject subject, string templateName, object model)
      : this(to, language, subject, templateName)
    {
      Model = model;
    }

    public string To { get; }
    public EmailMessageSubject Subject { get; }
    public string TemplateName { get; }
    public string Language { get; }
    public object Model { get; }
  }

  public class EmailMessageSubject
  {
    public EmailMessageSubject(string en, string pl)
    {
      EN = en;
      PL = pl;
    }

    public string EN { get; }
    public string PL { get; }
  }
}
