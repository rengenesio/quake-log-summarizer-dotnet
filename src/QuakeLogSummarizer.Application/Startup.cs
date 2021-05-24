using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using QuakeLogSummarizer.Core;
using QuakeLogSummarizer.Core.LogMessageParser;
using QuakeLogSummarizer.Infrastructure;

namespace QuakeLogSummarizer.Application
{
    /// <summary />
    public class Startup
    {
        private readonly Assembly _executingAssembly = Assembly.GetExecutingAssembly();
        
        private string AssemblyTitle => this._executingAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
        
        private string AssemblyDescription => this._executingAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

        private string AssemblyProduct => this._executingAssembly.GetCustomAttribute<AssemblyProductAttribute>().Product;

        /// <summary />
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = this.AssemblyTitle,
                    Description = this.AssemblyDescription,
                    Contact = new OpenApiContact()
                    {
                        Name = "Renan Rezende",
                        Url = new Uri("https://github.com/rengenesio")
                    }
                });

                string xmlFile = Path.Join(AppContext.BaseDirectory, $"{this.AssemblyProduct}.xml");
                c.IncludeXmlComments(xmlFile, true);
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

        /// <summary />
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quake Log Summarizer v1");
            });

            app.UseReDoc(c =>
            {
                c.RoutePrefix = "docs";
                c.DocumentTitle = $"{this.AssemblyTitle} - API Docs";
                c.SpecUrl = "/swagger/v1/swagger.json";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
