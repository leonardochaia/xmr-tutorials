using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace MoneroInteractive.WebApp.Localization
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUserLanguageService(
            this IServiceCollection services,
            LocalizationConfiguration config)
        {
            if (!config.AvailableLanguages.Any())
            {
                throw new InvalidOperationException(@"No languages provided.
AvailableLanguages and DefaultLanguage are required.");
            }

            services.AddSingleton(config);

            services.AddSingleton<IUserLanguageService>(serviceProvider =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                var logger = serviceProvider.GetRequiredService<ILogger<UserLanguageService>>();
                return new UserLanguageService(httpContextAccessor,
                    logger,
                    config.AvailableLanguages,
                    config.DefaultLanguage);
            });
        }
    }
}
