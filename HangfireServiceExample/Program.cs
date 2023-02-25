using Hangfire.InMemory;
using HangfireServiceExample.Impl;
using HangfireServiceExample.Impl.Infra;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HangfireServiceExample
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            using IHost host = Host
                .CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureLogging(b =>
                {
                    b.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.SingleLine = true;
                        options.TimestampFormat = "HH:mm:ss ";
                        options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled;
                    });
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<HangfireService>();
                    services.AddSingleton<InMemoryStorage>();
                    services.AddSingleton<IHangfireConfigurationService, HangfireConfigurationService>();
                    services.AddSingleton<IHangfireDashboardService, HangfireDashboardService>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
