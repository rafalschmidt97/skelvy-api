namespace Skelvy.Infrastructure.Notifications
{
  public class PushNotificationMessage
  {
    public PushNotificationMessage(string to, PushNotificationContent notification)
    {
      To = to;
      Notification = notification;
    }

    public PushNotificationMessage(string to, PushNotificationContent notification, object data)
      : this(to, notification)
    {
      Data = data;
    }

    public string To { get; }
    public PushNotificationContent Notification { get; }
    public object Data { get; }
  }
}
