using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace MoneroInteractive.WebApp.Localization
{
    public class LocalizationConfiguration
    {
        public string[] AvailableLanguages { get; set; }

        public string DefaultLanguage { get; set; } = "en";
    }
}
