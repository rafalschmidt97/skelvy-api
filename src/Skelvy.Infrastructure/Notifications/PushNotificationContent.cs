namespace Skelvy.Infrastructure.Notifications
{
  public class PushNotificationContent
  {
    public string Title { get; set; }
    public string Body { get; set; }
    public string TitleLocKey { get; set; }
    public string BodyLocKey { get; set; }
    public string Sound { get; } = "default";

    public static PushNotificationContent BuildInternational(string title, string body)
    {
      return new PushNotificationContent
      {
        TitleLocKey = title,
        BodyLocKey = body,
      };
    }

    public static PushNotificationContent Build(string title, string body)
    {
      return new PushNotificationContent
      {
        Title = title,
        Body = body,
      };
    }
  }
}
