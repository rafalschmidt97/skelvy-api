using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Notifications.Infrastructure;
using Skelvy.Application.Users.Infrastructure.Notifications;
using Skelvy.Domain.Enums;

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

    public async Task BroadcastUserCreated(UserCreatedNotification notification)
    {
      var message = new EmailMessage(
        notification.Email,
        notification.Language,
        new EmailMessageSubject("Your account has been created", "Twoje konto zostało utworzone"),
        "Created");

      await SendEmail(message);
    }

    public async Task BroadcastUserRemoved(UserRemovedNotification notification)
    {
      var message = new EmailMessage(
        notification.Email,
        notification.Language,
        new EmailMessageSubject("Your account has been deleted", "Twoje konto zostało usunięte"),
        "Removed");

      await SendEmail(message);
    }

    public async Task BroadcastUserDisabled(UserDisabledNotification notification)
    {
      dynamic model = new ExpandoObject();
      model.Reason = notification.Reason;

      var message = new EmailMessage(
        notification.Email,
        notification.Language,
        new EmailMessageSubject("Your account has been disabled", "Twoje konto zostało zablokowane"),
        "Disabled",
        model);

      await SendEmail(message);
    }

    private async Task SendEmail(EmailMessage message)
    {
      var body = await GetHtmlBody(message);

      var email = new MailMessage
      {
        From = new MailAddress(_configuration["SKELVY_EMAIL_USERNAME"], _configuration["SKELVY_EMAIL_NAME"]),
        To = { message.To },
        Subject = GeSubject(message),
        Body = body,
        IsBodyHtml = true,
      };

      using (var smtp = new SmtpClient(_configuration["SKELVY_EMAIL_HOST"], int.Parse(_configuration["SKELVY_EMAIL_PORT"]))
      {
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        Credentials = new NetworkCredential(_configuration["SKELVY_EMAIL_USERNAME"], _configuration["SKELVY_EMAIL_PASSWORD"]),
      })
      {
        await smtp.SendMailExAsync(email);
      }
    }

    private async Task<string> GetHtmlBody(EmailMessage message)
    {
      var path = message.Language != null ?
        $"Views/{message.Language}/{message.TemplateName}.cshtml" :
        $"Views/en/{message.TemplateName}.cshtml";

      var reader = new StreamReader(File.OpenRead(path));
      var template = await reader.ReadToEndAsync();

      return await _templateRenderer.ParseAsync(template, message.Model);
    }

    private static string GeSubject(EmailMessage message)
    {
      return message.Language == LanguageType.PL ? message.Subject.PL : message.Subject.EN;
    }
  }
}
