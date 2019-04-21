namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class EmailMessage
  {
    public EmailMessage(string to, string subject, string templateName)
    {
      To = to;
      Subject = subject;
      TemplateName = templateName;
    }

    public EmailMessage(string to, string subject, string templateName, object model)
      : this(to, subject, templateName)
    {
      Model = model;
    }

    public string To { get; }
    public string Subject { get; }
    public string TemplateName { get; }
    public object Model { get; }
  }
}
