using Google.Apis.Util;
using MailKit.Net.Smtp;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Models.Resources;
using May25.API.Core.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace May25.API.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;
        private readonly AppOptions _appOptions;
        private readonly IHtmlService _htmlService;

        public EmailService(IOptions<EmailOptions> emailOptions, IOptions<AppOptions> appOptions,
            IHtmlService htmlService)
        {
            _emailOptions = emailOptions.Value.ThrowIfNull(nameof(emailOptions));
            _appOptions = appOptions.Value.ThrowIfNull(nameof(appOptions));
            _htmlService = htmlService;
        }

        public async Task SendAsync(string recipients, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_appOptions.Name, _emailOptions.Username));

            foreach (var recipient in recipients.Split(','))
            {
                message.To.Add(new MailboxAddress(recipient, recipient));
            }

            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_emailOptions.SmtpClient, _emailOptions.SmtpPort, true);
            await smtpClient.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password);
            await smtpClient.SendAsync(message);
            await smtpClient.DisconnectAsync(true);
        }

        public async Task SendConfirmationEmailAsync(ConfirmEmailParamsDTO data)
        {
            await SendAsync(data.Email, "Confirmación de email", _htmlService.GetConfirmationEmail(data));
        }

        public async Task SendNotificationEmailAsync(string recipient, NotificationForCreationDTO nfc)
        {
            await SendAsync(recipient, "Notificación", _htmlService.GetNotificationEmail(recipient, nfc.Title, nfc.Body));
        }

        public async Task SendPasswordResetRequestAsync(string email, string token)
        {
            await SendAsync(email, "Recuperación de contraseña", _htmlService.GetPasswordResetRequest(email, token));
        }
    }
}
