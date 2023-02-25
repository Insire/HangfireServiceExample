using Microsoft.Extensions.Hosting;

namespace HangfireServiceExample.Impl.Infra
{
    public sealed class HangfireService : IHostedService
    {
        private readonly IHangfireConfigurationService _hangfireConfigurationService;
        private readonly IHangfireDashboardService _hangfireDashboardService;

        public HangfireService(IHangfireConfigurationService hangfireConfigurationService, IHangfireDashboardService hangfireDashboardService)
        {
            _hangfireConfigurationService = hangfireConfigurationService;
            _hangfireDashboardService = hangfireDashboardService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(_hangfireConfigurationService.StartAsync(cancellationToken), _hangfireDashboardService.StartAsync(cancellationToken));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(_hangfireConfigurationService.StopAsync(cancellationToken), _hangfireDashboardService.StopAsync(cancellationToken));
        }
    }
}
