using May25.API.Core.Contracts.Services;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Resources;
using May25.API.Core.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace May25.API.Core.Services
{
    public class HtmlService : IHtmlService
    {
        private readonly string _htmlFolder;
        private readonly AppOptions _appOptions;
        private readonly IUrlService _urlService;

        public HtmlService(IWebHostEnvironment environment, IOptions<AppOptions> appOptions, IUrlService urlService)
        {
            _htmlFolder = Path.Combine(environment.WebRootPath, "html");
            _appOptions = appOptions.Value.ThrowIfNull(nameof(appOptions));
            _urlService = urlService.ThrowIfNull(nameof(urlService));
        }

        public string GetEmailConfirmationError(string email)
        {
            var content = File.ReadAllText(Path.Combine(_htmlFolder, "email-confirmation-error.html"));

            return CommonReplaces(content, email);
        }

        public string GetEmailConfirmationSuccess(string email)
        {
            var content = File.ReadAllText(Path.Combine(_htmlFolder, "email-confirmed.html"));

            return CommonReplaces(content, email);
        }

        public string GetEmailUnsubscribed()
        {
            var content = File.ReadAllText(Path.Combine(_htmlFolder, "email-unsubscribed.html"));

            return CommonReplaces(content, string.Empty);
        }

        public string GetConfirmationEmail(ConfirmEmailParamsDTO data)
        {
            var content = File.ReadAllText(Path.Combine(_htmlFolder, "confirmation-email-template.html"));

            return CommonReplaces(content, data.Email)
                .Replace("@@ConfirmationUrl@@", _urlService.GetConfirmationUrl(data));
        }

        public string GetNotificationEmail(string email, string title, string body)
        {
            var content = File.ReadAllText(Path.Combine(_htmlFolder, "notification-email-template.html"));

            return CommonReplaces(content, email)
                .Replace("@@NotificationTitle@@", title)
                .Replace("@@NotificationBody@@", body);
        }

        public string GetPasswordResetRequest(string email, string token)
        {
            var content = File.ReadAllText(Path.Combine(_htmlFolder, "password-reset-request-template.html"));

            return CommonReplaces(content, email)
                .Replace("@@PasswordResetUrl@@", _urlService.GetPasswordResetUrl(email, token));
        }

        public string GetPasswordResetForm(string email, string token)
        {
            var content = File.ReadAllText(Path.Combine(_htmlFolder, "password-reset-form.html"));

            return CommonReplaces(content, email)
                .Replace("@@ResetPasswordUrl@@", _urlService.GetPasswordResetUrl(email, token));
        }

        private string CommonReplaces(string content, string email)
        {
            return content
                .Replace("@@AppName@@", _appOptions.Name)
                .Replace("@@AppLogoBase64@@", _appOptions.LogoBase64)
                .Replace("@@AppSite@@", new Uri(_appOptions.Site, UriKind.Absolute).ToString())
                .Replace("@@UnsubscribeUrl@@", _urlService.GetUnsubscribeUrl(email));
        }
    }
}
