using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace xmr_tutorials.Daemon
{
    public static class IServiceCollectionDaemonExtensions
    {
        public static IServiceCollection AddDaemon(
            this IServiceCollection services,
            IConfiguration config)
        {

            services.Configure<DaemonConfiguration>(options => config.GetSection("Daemon").Bind(options));

            // services.AddSingleton<DaemonManager>();

            return services;
        }
    }
}