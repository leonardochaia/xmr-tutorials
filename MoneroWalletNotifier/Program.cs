using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoneroClient.Wallet;

namespace MoneroWalletNotifier
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = ConfigureServices();

            var logger = services
                .GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            var wallet = services.GetService<WalletManager>();
            var address = await wallet.QueryAddressAsync();
            logger.LogInformation($"Wallet {address}");

        }

        static IServiceProvider ConfigureServices()
        {
            var configuration = BindConfiguration();

            var walletConfig = new WalletConfiguration();
            configuration.GetSection("Wallet").Bind(walletConfig);

            var serviceCollection = new ServiceCollection()
                .AddLogging(opt =>
                {
                    opt.AddConsole();
                    opt.SetMinimumLevel(LogLevel.Trace);
                })
                .AddWallet(walletConfig);

            return serviceCollection.BuildServiceProvider();
        }

        static IConfiguration BindConfiguration()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                        .AddJsonFile("appsettings.Development.json", optional: true)
                        .AddJsonFile("appsettings.json", optional: true);

            IConfigurationRoot configuration = builder.Build();
            Console.WriteLine(Path.Combine(AppContext.BaseDirectory));
            return configuration;
        }
    }
}
