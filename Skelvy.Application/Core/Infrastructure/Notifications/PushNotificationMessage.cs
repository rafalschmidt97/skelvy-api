using System.Collections.Generic;

namespace Skelvy.Application.Core.Infrastructure.Notifications
{
  public class PushNotificationMessage
  {
    public PushNotificationContent Notification { get; set; }
    public object Data { get; set; }
    public ICollection<string> RegistrationIds { get; set; }
  }

  public class PushNotificationContent
  {
    public string Title { get; set; }
    public string Body { get; set; }
  }
}
