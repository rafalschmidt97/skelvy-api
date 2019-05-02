using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microsoft.Extensions.Configuration;
using Skelvy.Application.Notifications;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;

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

    public async Task BroadcastUserCreated(User user)
    {
      var message = new EmailMessage(
        user.Email,
        user.Language,
        new EmailMessageSubject("Your account has been created", "Twoje konto zostało utworzone"),
        "Created");

      await SendEmail(message);
    }

    public async Task BroadcastUserRemoved(User user)
    {
      var message = new EmailMessage(
        user.Email,
        user.Language,
        new EmailMessageSubject("Your account has been deleted", "Twoje konto zostało usunięte"),
        "Removed");

      await SendEmail(message);
    }

    public async Task BroadcastUserDisabled(User user, string reason)
    {
      dynamic model = new ExpandoObject();
      model.Reason = reason;

      var message = new EmailMessage(
        user.Email,
        user.Language,
        new EmailMessageSubject("Your account has been disabled", "Twoje konto zostało zablokowane"),
        "Disabled",
        model);

      await SendEmail(message);
    }

    private async Task SendEmail(EmailMessage message)
    {
      var body = await GetHtmlBody(message);

      var email = new MailMessage()
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
      string template;

      var path = message.Language != null ?
        $"Views/{message.Language}/{message.TemplateName}.cshtml" :
        $"Views/en/{message.TemplateName}.cshtml";

      using (var reader = new StreamReader(File.OpenRead(path)))
      {
        template = await reader.ReadToEndAsync();
      }

      return await _templateRenderer.ParseAsync(template, message.Model);
    }

    private static string GeSubject(EmailMessage message)
    {
      return message.Language == LanguageTypes.PL ? message.Subject.PL : message.Subject.EN;
    }
  }
}
