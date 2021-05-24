using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using QuakeLogSummarizer.Core;
using QuakeLogSummarizer.Core.LogMessageParser;
using QuakeLogSummarizer.Infrastructure;

namespace QuakeLogSummarizer.Application
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = executingAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title,
                    Description = executingAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description,
                    Contact = new OpenApiContact()
                    {
                        Name = "Renan Rezende",
                        Url = new Uri("https://github.com/rengenesio")
                    }
                });
            });

            services.AddSingleton<LogSummarizer>()
                .AddSingleton<ILogMessageExtractor, LogMessageExtractor>()
                .AddSingleton<ILogMessageParser, InitGameLogMessageParser>()
                .AddSingleton<ILogMessageParser, ClientConnectLogMessageParser>()
                .AddSingleton<ILogMessageParser, ClientUserInfoChangedLogMessageParser>()
                .AddSingleton<ILogMessageParser, KillLogMessageParser>()
                .AddScoped<ILogFileReader, LogFileReader>()
                .AddScoped<IGameEventsProcessor, GameEventsProcessor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quake Log aaaaaaaaSummarizer v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
