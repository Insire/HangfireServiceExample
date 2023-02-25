using Hangfire;
using Hangfire.InMemory;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HangfireServiceExample.Impl.Infra
{
    [RegisterSingleton]
    public sealed class HangfireDashboardService : IHangfireDashboardService
    {
        private readonly IConfiguration _configuration;
        private readonly InMemoryStorage _inMemoryStorage;

        private IWebHost? _webHost;

        public HangfireDashboardService(IConfiguration configuration, InMemoryStorage inMemoryStorage)
        {
            _configuration = configuration;
            _inMemoryStorage = inMemoryStorage;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _webHost = WebHost
                .CreateDefaultBuilder()
                .UseConfiguration(_configuration)
                .UseStartup(c => new Startup(_inMemoryStorage))
                .UseUrls("https://localhost:5000")
                .Build();

            return _webHost.RunAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var webHost = _webHost;
            if (webHost != null)
            {
                await webHost.StopAsync(cancellationToken);
                webHost.Dispose();
            }

            _webHost = null;
        }

        private sealed class Startup
        {
            private readonly InMemoryStorage _inMemoryStorage;

            public Startup(InMemoryStorage inMemoryStorage)
            {
                _inMemoryStorage = inMemoryStorage;
            }

            public void Configure(IApplicationBuilder app)
            {
                app.UseStaticFiles();
                app.UseRouting();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHangfireDashboard();
                });

                app.UseHangfireDashboard(options: new DashboardOptions()
                {
                    DashboardTitle = "JobServer",
                    DisplayStorageConnectionString = false,
                    //DisplayNameFunc
                });
            }

            public void ConfigureServices(IServiceCollection services)
            {
                services.AddHangfire(config =>
                {
                    config
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseStorage(_inMemoryStorage);
                });
            }
        }
    }
}
