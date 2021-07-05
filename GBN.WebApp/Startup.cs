using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using UnivIntel.GBN.Core;
using UnivIntel.GBN.Core.DataAccess.Entities;
using UnivIntel.GBN.Core.Handlers;
using UnivIntel.GBN.Core.Services;
using UnivIntel.GBN.WebApp.Middlewares;

namespace GBN.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            AuthentificationService.FillTokens();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IQueryExecutionFactory, QueryExecutionFactory>();
            services.AddTransient<IAuthentificationService, AuthentificationService>();
            services.AddTransient<IBCryptProvider, BCryptProvider>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IGeocodingService, GeocodingService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                //logging crashes on production
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var error = exceptionHandlerPathFeature.Error;
                        if (error != null)
                        {
                            try
                            {
                                await new DatabaseService(GlobalSettings.ConnStr, "crashlogger").AddAsync(
                                    new CrashLog
                                    {
                                        Path = exceptionHandlerPathFeature.Path,
                                        Message = error.Message,
                                        StackTrace = error.StackTrace,
                                        InnerMessage = error.InnerException?.Message,
                                        InnerStackTrace = error.InnerException?.StackTrace,
                                    },
                                    new List<string> { "Id" }
                                );
                            }
                            catch
                            {
                                //sorry, but the code after it should work, even if the registration error failed
                            }
                        }

                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                        await context.Response.WriteAsync("ERROR!<br><br>\r\n");
                        await context.Response.WriteAsync("<a href=\"/\">Home</a><br>\r\n");
                        await context.Response.WriteAsync("</body></html>\r\n");
                        await context.Response.WriteAsync(new string(' ', 512)); // IE padding
                    });
                });
            }

            app.UseRouting();

            app.UseMiddleware<UserSessionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
