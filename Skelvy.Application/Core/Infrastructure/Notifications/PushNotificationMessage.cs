using System.Collections.Generic;

namespace Skelvy.Application.Core.Infrastructure.Notifications
{
  public class PushNotificationMessage
  {
    public PushNotificationContent Notification { get; set; }
    public ICollection<string> RegistrationIds { get; set; }
    public object Data { get; set; }
    public bool DryRun { get; set; }
  }

  public class PushNotificationContent
  {
    public string Title { get; set; }
    public string Body { get; set; }
    public string Sound { get; set; } = "default";
  }

  public class PushVerification
  {
    public int Success { get; set; }
    public int Failure { get; set; }
    public ICollection<PushVerificationResult> Results { get; set; }
  }

  public class PushVerificationResult
  {
    public string MessageId { get; set; }
    public string Error { get; set; }
  }
}
