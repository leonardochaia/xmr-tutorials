using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MoneroInteractive.WebApp.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoneroInteractive.WebApp.SpaExtensions
{
    public static class AppBuilderExtensions
    {
        public static void MapSpaForLanguages(
            this IApplicationBuilder app,
            IHostingEnvironment env,
            IEnumerable<string> languages)
        {
            app.Use((context, next) =>
            {
                // If the Request URL contains no language
                // attempt to get a language from the request
                // and redirect to the proper language ui
                var path = context.Request.Path;
                if (!languages.Any(lang => path.StartsWithSegments($"/{lang}")))
                {
                    var languageService = context.RequestServices.GetRequiredService<IUserLanguageService>();
                    context.Response.Redirect($"/{languageService.GetUserLocale()}{path}", permanent: false);
                    return Task.CompletedTask;
                }

                return next();
            });

            foreach (var lang in languages)
            {
                app.MapSpa(env, new SpaBranchOptions(lang));
            }
        }

        public static void MapSpa(
            this IApplicationBuilder app,
            IHostingEnvironment env,
            SpaBranchOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // branch middleware to a app-specific path
            app.Map(options.MapPath, app1 =>
            {
                // we only need SPA static files in prod mode
                // don't use AddSpaStaticFiles -- it puts a Singleton file provider in the container
                var fileOptions = new StaticFileOptions();
                if (!env.IsDevelopment())
                {
                    // path should be dist folder of the SPA
                    // this will error if ng prod build has not been run
                    var staticPath = Path.Combine(Directory.GetCurrentDirectory(), $"{options.SourcePath}{options.DistPath}");
                    fileOptions.FileProvider = new PhysicalFileProvider(staticPath);

                    // this will root in the MapPath since we are branched
                    app1.UseSpaStaticFiles(options: fileOptions);
                }

                // create the SPA within the branch path
                app1.UseSpa(spa =>
                {
                    spa.Options.SourcePath = options.SourcePath;
                    spa.Options.DefaultPage = options.DefaultPage;

                    if (options.EnableServerSideRendering)
                    {
                        spa.UseSpaPrerendering(renderingOptions =>
                        {
                            renderingOptions.BootModulePath = $"{spa.Options.SourcePath}{options.ServerRenderBundlePath}";
                            renderingOptions.BootModuleBuilder = env.IsDevelopment()
                                ? new AngularCliBuilder(npmScript: options.ServerRenderBuildScript)
                                : null;
                            // proxied socksjs-node will be under the branched path
                            renderingOptions.ExcludeUrls = new[] { $"{options.MapPath}/sockjs-node" };
                        });
                    }

                    if (env.IsDevelopment())
                    {
                        // this defaults to start:hosted, which has some ng serve options for multi-spa
                        spa.UseAngularCliServer(npmScript: options.DevServerScript);
                    }
                    else
                    {
                        // ensure the DefaultPage is found within the app
                        spa.Options.DefaultPageStaticFileOptions = fileOptions;
                    }
                });
            });
        }
    }
}
