using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace MoneroInteractive.WebApp.Localization
{
    public class UserLanguageService : IUserLanguageService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<UserLanguageService> logger;
        private readonly IEnumerable<string> availableLanguages;
        private readonly string defaultLanguage;

        public UserLanguageService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserLanguageService> logger,
            IEnumerable<string> availableLanguages,
            string defaultLanguage)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.availableLanguages = availableLanguages
                .Select(l => l.ToLowerInvariant());
            this.defaultLanguage = defaultLanguage;

            logger.LogInformation($@"LanguageService initialized.
Available languages: {string.Join(',', availableLanguages)}
Default Language: {defaultLanguage}");
        }

        public string GetUserLocale()
        {
            var request = httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                logger.LogDebug($"Request was null, return default language: {defaultLanguage}");
                return defaultLanguage;
            }

            var cookieLocale = request.Cookies[".User.Locale"];
            if (!string.IsNullOrWhiteSpace(cookieLocale)
                && availableLanguages.Contains(cookieLocale))
            {
                logger.LogInformation($"Found valid language in Cookies: {cookieLocale}");
                return cookieLocale;
            }

            var userLocales = request.Headers["Accept-Language"].ToString();
            var userAcceptLanguage = GetAcceptLanguageFromHeaderOrNull(userLocales);

            if (!string.IsNullOrWhiteSpace(userAcceptLanguage))
            {
                logger.LogInformation($"Found valid language in Accept-Language Header: {userAcceptLanguage}");
                return userAcceptLanguage;
            }

            logger.LogInformation($"Couldn't determine language. Using default: {defaultLanguage}");
            return defaultLanguage;
        }

        public string GetAcceptLanguageFromHeaderOrNull(string headerValue)
        {
            if (headerValue == null)
            {
                return null;
            }

            try
            {
                var clientLanguages = headerValue
                    .Split(',')
                    .Select(StringWithQualityHeaderValue.Parse)
                    .OrderByDescending(language => language.Quality.GetValueOrDefault(1))
                    .Select(language => language.Value)
                    .Select(languageCode =>
                    {
                        if (languageCode.Contains("-"))
                        {
                            return languageCode.Split('-').First();
                        }

                        return languageCode;
                    })
                    .Select(languageCode => languageCode.ToLowerInvariant())
                    .Distinct()
                    .Where(languageCode => !string.IsNullOrWhiteSpace(languageCode) && languageCode.Trim() != "*");
                return clientLanguages
                    .FirstOrDefault(clientLanguage => availableLanguages.Contains(clientLanguage));
            }
            catch
            {
                return null;
            }
        }
    }
}
