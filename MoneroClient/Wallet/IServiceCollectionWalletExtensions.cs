using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MoneroClient.Rpc;

namespace MoneroClient.Wallet
{
    public static class IServiceCollectionWalletExtensions
    {
        public static IServiceCollection AddWallet(
            this IServiceCollection services,
            WalletConfiguration config)
        {

            services.AddSingleton(config);

            services.TryAddSingleton<RpcClient>();

            services.AddSingleton<WalletManager>();

            return services;
        }
    }
}