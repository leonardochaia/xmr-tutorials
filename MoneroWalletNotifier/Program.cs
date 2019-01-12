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

            var splitter = services.GetService<WalletOutputsDicer>();
            await splitter.TryDice();
            logger.LogInformation("Application Finished");
            Console.ReadKey();
        }

        static IServiceProvider ConfigureServices()
        {
            var configuration = BindConfiguration();

            var services = new ServiceCollection();

            services.AddLogging(opt =>
                {
                    opt.AddConsole();
                    opt.SetMinimumLevel(LogLevel.Trace);
                });

            var walletConfig = new WalletConfiguration();
            configuration.GetSection("Wallet").Bind(walletConfig);
            services.AddWallet(walletConfig);
            services.Configure<DicingConfiguration>(configuration.GetSection("Dicing"));
            services.AddSingleton<WalletOutputsDicer>();

            return services.BuildServiceProvider();
        }

        static IConfiguration BindConfiguration()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddJsonFile("appsettings.Development.json", optional: false);

            IConfigurationRoot configuration = builder.Build();
            return configuration;
        }
    }
}
