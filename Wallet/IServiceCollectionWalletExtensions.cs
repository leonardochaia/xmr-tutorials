using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace xmr_tutorials.Wallet
{
    public static class IServiceCollectionWalletExtensions
    {
        public static IServiceCollection AddWallet(
            this IServiceCollection services,
            IConfiguration config)
        {

            services.Configure<WalletConfiguration>(options => config.GetSection("Wallet").Bind(options));

            services.AddSingleton<WalletManager>();

            return services;
        }
    }
}