namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class SocketNotificationMessage
  {
    public SocketNotificationMessage(SocketNotificationContent notification, string type, SocketNotificationData data)
    {
      Notification = notification;
      Type = type;
      Data = data;
    }

    public SocketNotificationContent Notification { get; }
    public string Type { get; }
    public SocketNotificationData Data { get; }
  }

  public class SocketNotificationContent
  {
    public string Title { get; set; }
    public string Body { get; set; }
    public string TitleLocKey { get; set; }
    public string BodyLocKey { get; set; }
  }

  public class SocketNotificationData
  {
    public string Action { get; set; }
    public string RedirectTo { get; set; }
    public object Data { get; set; }
  }
}
