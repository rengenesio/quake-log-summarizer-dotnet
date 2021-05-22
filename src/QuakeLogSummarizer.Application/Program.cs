using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuakeLogSummarizer.Core;
using QuakeLogSummarizer.Core.LogMessageParser;

namespace QuakeLogSummarizer.Application
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            LogSummarizer summarizer = host.Services.GetRequiredService<LogSummarizer>();
            await summarizer.Summarize(args[0]);
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddSingleton<ILogMessageExtractor, LogMessageExtractor>()
                        .AddSingleton<ILogMessageParser, ClientConnectLogMessageParser>()
                        .AddSingleton<ILogMessageParser, InitGameLogMessageParser>()
                        .AddSingleton<LogSummarizer>());
        }
    }
}
