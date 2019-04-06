using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Infrastructure.Notifications;
using Skelvy.Domain.Entities;

namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class EmailNotificationsService : IEmailNotificationsService
  {
    private readonly IConfiguration _configuration;
    private readonly ITemplateRenderer _templateRenderer;

    public EmailNotificationsService(IConfiguration configuration, ITemplateRenderer templateRenderer)
    {
      _configuration = configuration;
      _templateRenderer = templateRenderer;
    }

    public async Task BroadcastUserCreated(User user, CancellationToken cancellationToken)
    {
      var message = new EmailMessage
      {
        To = user.Email,
        Subject = "Account has been created",
        TemplateName = "Created"
      };

      await SendEmail(message, cancellationToken);
    }

    public async Task BroadcastUserDeleted(User user, CancellationToken cancellationToken)
    {
      var message = new EmailMessage
      {
        To = user.Email,
        Subject = "Account has been deleted",
        TemplateName = "Deleted"
      };

      await SendEmail(message, cancellationToken);
    }

    public async Task BroadcastUserDisabled(User user, string reason, CancellationToken cancellationToken)
    {
      dynamic model = new ExpandoObject();
      model.Reason = reason;

      var message = new EmailMessage
      {
        To = user.Email,
        Subject = "Account has been disabled",
        TemplateName = "Disabled",
        Model = model
      };

      await SendEmail(message, cancellationToken);
    }

    private async Task SendEmail(EmailMessage message, CancellationToken cancellationToken)
    {
      var body = await GetHtmlBody(message);

      var email = new MailMessage
      {
        From = new MailAddress(_configuration["Email:Username"], _configuration["Email:Name"]),
        To = { message.To },
        Subject = message.Subject,
        Body = body,
        IsBodyHtml = true
      };

      using (var smtp = new SmtpClient(_configuration["Email:Host"], int.Parse(_configuration["Email:Port"]))
      {
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"])
      })
      {
        await smtp.SendMailExAsync(email, cancellationToken);
      }
    }

    private async Task<string> GetHtmlBody(EmailMessage message)
    {
      string template;

      using (var reader = new StreamReader(File.OpenRead($"Views/{message.TemplateName}.cshtml")))
      {
        template = await reader.ReadToEndAsync();
      }

      return await _templateRenderer.ParseAsync(template, message.Model);
    }
  }

  public class EmailMessage
  {
    public string To { get; set; }
    public string Subject { get; set; }
    public string TemplateName { get; set; }
    public object Model { get; set; }
  }
}
