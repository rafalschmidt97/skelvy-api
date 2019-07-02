namespace Skelvy.Infrastructure.Notifications
{
  public class PushNotificationMessage
  {
    public PushNotificationMessage(string to, PushNotificationContent notification)
    {
      To = to;
      Notification = notification;
    }

    public PushNotificationMessage(string to, PushNotificationContent notification, PushNotificationData data)
      : this(to, notification)
    {
      Data = data;
    }

    public string To { get; }
    public PushNotificationContent Notification { get; }
    public PushNotificationData Data { get; }
  }

  public class PushNotificationContent
  {
    public string Title { get; set; }
    public string Body { get; set; }
    public string TitleLocKey { get; set; }
    public string BodyLocKey { get; set; }
    public string Sound { get; } = "default";
  }

  public class PushNotificationData
  {
    public string Action { get; set; }
    public string RedirectTo { get; set; }
    public object Data { get; set; }
  }
}
