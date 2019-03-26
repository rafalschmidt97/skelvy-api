using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FluentEmail.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Skelvy.WebAPI.Infrastructure.Emails;

namespace Skelvy.WebAPI.Extensions
{
  public static class EmailExtension
  {
    public static void AddEmail(this IServiceCollection services, IConfiguration configuration)
    {
      services
        .AddFluentEmail(configuration["Email:Username"], configuration["Email:Name"])
        .AddSmtpSender(PrepareSmtpClientSsl(configuration));

      services.TryAdd(ServiceDescriptor.Singleton<IRazorViewToStringRenderer, RazorViewToStringRenderer>());
      services.TryAdd(ServiceDescriptor.Singleton<ITemplateRenderer, RazorFixRenderer>());
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

  public class RazorFixRenderer : ITemplateRenderer
  {
    private readonly IRazorViewToStringRenderer _renderer;

    public RazorFixRenderer(IRazorViewToStringRenderer renderer)
    {
      _renderer = renderer;
    }

    public async Task<string> ParseAsync<T>(string template, T model, bool isHtml = true)
    {
      return await _renderer.RenderViewToStringAsync(template, model);
    }

    public string Parse<T>(string template, T model, bool isHtml = true)
    {
      return ParseAsync(template, model, isHtml).GetAwaiter().GetResult();
    }
  }
}
