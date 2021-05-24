using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace QuakeLogSummarizer.Application
{
    /// <summary />
    public class Program
    {
        /// <summary />
        public static void Main(string[] args)
        {

            Program.CreateHostBuilder(args)
                .Build()
                .Run();
        }

        /// <summary />
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://*:5000");
                });
    }
}
