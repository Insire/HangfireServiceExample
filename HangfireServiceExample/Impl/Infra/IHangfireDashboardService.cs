namespace HangfireServiceExample.Impl.Infra
{
    public interface IHangfireDashboardService
    {
        Task StartAsync(CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);
    }
}
