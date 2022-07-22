using May25.API.Core.Contracts.Services;
using May25.API.Core.Models.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace May25.API.Core.Services
{
    public class UrlService : IUrlService
    {
        private readonly string _baseUrl;

        public UrlService(IHttpContextAccessor httpContextAccessor, ILogger<UrlService> logger)
        {
            _baseUrl = httpContextAccessor.HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host.Value;
        }

        public string GetConfirmationUrl(ConfirmEmailParamsDTO data)
        {
            return new Uri($"{_baseUrl}/api/users/confirm-email?email={data.Email}&token={data.Token}", UriKind.Absolute).ToString();
        }

        public string GetPasswordResetUrl(string email, string token)
        {
            return new Uri($"{_baseUrl}/api/users/password-reset?email={email}&token={token}", UriKind.Absolute).ToString();
        }

        public string GetUnsubscribeUrl(string email)
        {
            return new Uri($"{_baseUrl}/api/users/unsubscribe?email={email}", UriKind.Absolute).ToString();
        }
    }
}
