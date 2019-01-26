using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MoneroClient.Wallet;
using MoneroInteractive.WebApp.AppStatus;
using MoneroInteractive.WebApp.Localization;
using MoneroInteractive.WebApp.SpaExtensions;
using MoneroInteractive.WebApp.Utils;

namespace MoneroInteractive.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var walletConfig = Configuration.GetSection("Wallet")
                .Get<WalletConfiguration>();
            services.AddWallet(walletConfig);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Monero Tutorials API",
                    Version = "v1"
                });
            });

            services.AddSignalR();
            services.AddSingleton<AppStatusHubBroadcaster>();

            services.AddHttpContextAccessor();

            var localizationConfig = Configuration.GetSection("Localization")
                .Get<LocalizationConfiguration>();
            services.AddUserLanguageService(localizationConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            LocalizationConfiguration localizationConfig)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Monero Tutorials V1");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<AppStatusHub>("/api/hubs/app-status");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.MapSpaForLanguages(env, localizationConfig.AvailableLanguages);

            var timers = new AsyncSafeTimer[] {
                app.ApplicationServices.GetRequiredService<AppStatusHubBroadcaster>(),
            };

            foreach (var timer in timers)
            {
                timer.StartTimer();
            }
        }
    }
}
