using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Notifications.Infrastructure;
using Skelvy.Application.Users.Infrastructure.Notifications;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Enums;

namespace Skelvy.WebAPI.Infrastructure.Notifications
{
  public class EmailNotificationsService : IEmailNotificationsService
  {
    private readonly IConfiguration _configuration;
    private readonly ITemplateRenderer _templateRenderer;
    private readonly ILogger<EmailNotificationsService> _logger;

    public EmailNotificationsService(IConfiguration configuration, ITemplateRenderer templateRenderer, ILogger<EmailNotificationsService> logger)
    {
      _configuration = configuration;
      _templateRenderer = templateRenderer;
      _logger = logger;
    }

    public async Task BroadcastUserCreated(UserCreatedNotification notification)
    {
      var message = new EmailMessage(
        notification.Email,
        notification.Language,
        LanguageType.Switch(
          notification.Language,
          "Your account has been created",
          "Twoje konto zostało utworzone",
          "Dein Account wurde erstellt",
          "Tu cuenta ha sido creada"),
        "Created");

      await SendEmail(message);
    }

    public async Task BroadcastUserRemoved(UserRemovedNotification notification)
    {
      var message = new EmailMessage(
        notification.Email,
        notification.Language,
        LanguageType.Switch(
          notification.Language,
          "Your account has been deleted",
          "Twoje konto zostało usunięte",
          "Dein Account wurde gelöscht",
          "Tu cuenta ha sido eliminada"),
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
        LanguageType.Switch(
          notification.Language,
          "Your account has been disabled",
          "Twoje konto zostało zablokowane",
          "Dein Account wurde blockiert",
          "Tu cuenta ha sido desactivada"),
        "Disabled",
        model);

      await SendEmail(message);
    }

    public async Task BroadcastCustomMessage(CustomEmailNotification notification)
    {
      dynamic model = new ExpandoObject();
      model.Message = notification.Message;

      var message = new EmailMessage(
        notification.To,
        notification.Language,
        notification.Subject,
        "Custom",
        model);

      await SendEmail(message);
    }

    private async Task SendEmail(EmailMessage message)
    {
      try
      {
        var body = await GetHtmlBody(message);

        var email = new MailMessage
        {
          From = new MailAddress(_configuration["SKELVY_EMAIL_USERNAME"], _configuration["SKELVY_EMAIL_NAME"]),
          To = { message.To },
          Subject = message.TranslatedSubject,
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
      catch (Exception exception)
      {
        _logger.LogCritical(exception, "Unexpected Server Exception while sending an email:");
      }
    }

    private async Task<string> GetHtmlBody(EmailMessage message)
    {
      var path = $"Skelvy.WebAPI.Views.{message.Language}.{message.TemplateName}.cshtml";
      var template = GetResourceAsString(GetType().GetTypeInfo().Assembly, path);
      return await _templateRenderer.ParseAsync(template, message.Model);
    }

    private static string GetResourceAsString(Assembly assembly, string path)
    {
      string result;

      using (var stream = assembly.GetManifestResourceStream(path))
      using (var reader = new StreamReader(stream ?? throw new InternalServerErrorException("Could not resolve email template")))
      {
        result = reader.ReadToEnd();
      }

      return result;
    }
  }
}
