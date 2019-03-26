using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Skelvy.WebAPI.Extensions
{
  public static class EmailExtension
  {
    public static void AddEmail(this IServiceCollection services, IConfiguration configuration)
    {
      services
        .AddFluentEmail(configuration["Email:Username"], configuration["Email:Name"])
        .AddSmtpSender(PrepareSmtpClientSsl(configuration))
        .AddRazorRenderer();
    }

    private static SmtpClient PrepareSmtpClientSsl(IConfiguration configuration)
    {
      return new SmtpClient(configuration["Email:Host"], int.Parse(configuration["Email:Port"]))
      {
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        Credentials = new NetworkCredential(configuration["Email:Username"], configuration["Email:Password"])
      };
    }
  }
}
