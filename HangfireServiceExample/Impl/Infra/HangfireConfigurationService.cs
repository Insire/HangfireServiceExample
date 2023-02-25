using Hangfire;
using Hangfire.InMemory;
using HangfireServiceExample.Impl.Metrics;
using HangfireServiceExample.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HangfireServiceExample.Impl.Infra
{
    public sealed class HangfireConfigurationService : IHangfireConfigurationService
    {
        private readonly InMemoryStorage _inMemoryStorage;

        private IHost? _host;

        public HangfireConfigurationService(InMemoryStorage inMemoryStorage)
        {
            _inMemoryStorage = inMemoryStorage;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _host = Host
                .CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddScoped<IDbContextFactory, DbContextFactory>();
                    services.AddScoped<IDbContextUnitOfWork, DbContextUnitOfWork>();
                    services.AddScoped(c =>
                    {
                        var factory = c.GetRequiredService<IDbContextUnitOfWork>();

                        return factory.Create();
                    });

                    services.AutoRegister();

                    services.AddHangfire(c =>
                    {
                        c
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseStorage(_inMemoryStorage)
                        .UseFilter(new HangfireCountersFilter());
                    });

                    services.AddHangfireServer(o => o.WorkerCount = 1);
                })
                .Build();

            await using (var scope = _host.Services.CreateAsyncScope())
            {
                var c = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();
                c.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

                var f = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                f.RegisterJobs();
            }

            await _host.RunAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var host = _host;
            if (host != null)
            {
                host?.Dispose();
            }

            _host = null;

            return Task.CompletedTask;
        }
    }
}
