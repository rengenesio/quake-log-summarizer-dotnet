using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuakeLogSummarizer.Core;
using QuakeLogSummarizer.Core.LogMessageParser;
using QuakeLogSummarizer.Core.Model;
using QuakeLogSummarizer.Core.Model.Output;

namespace QuakeLogSummarizer.Application
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            LogSummarizer summarizer = host.Services.GetRequiredService<LogSummarizer>();
            IEnumerable<Game> gameList = await summarizer.Summarize(args[0]);

            LogSummary summary = new LogSummary(gameList);

            Console.WriteLine(JsonSerializer.Serialize(summary, new JsonSerializerOptions()
            {
                WriteIndented = true
            }));
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddSingleton<ILogMessageExtractor, LogMessageExtractor>()
                        .AddSingleton<ILogMessageParser, InitGameLogMessageParser>()
                        .AddSingleton<ILogMessageParser, ClientConnectLogMessageParser>()
                        .AddSingleton<ILogMessageParser, ClientUserInfoChangedLogMessageParser>()
                        .AddSingleton<ILogMessageParser, KillLogMessageParser>()
                        .AddSingleton<LogSummarizer>());
        }
    }
}
