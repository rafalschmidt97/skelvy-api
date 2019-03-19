using System.Collections.Generic;

namespace Skelvy.Application.Core.Infrastructure.Notifications
{
  public class PushNotificationMessage
  {
    public string To { get; set; }
    public ICollection<string> RegistrationIds { get; set; }
    public PushNotificationContent Notification { get; set; }
    public object Data { get; set; }
  }

  public class PushNotificationContent
  {
    public string Title { get; set; }
    public string Body { get; set; }
    public string Sound { get; set; } = "default";
  }
}
