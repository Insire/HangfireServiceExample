namespace HangfireServiceExample.Impl.Infra
{
    public interface IHangfireConfigurationService
    {
        Task StartAsync(CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);
    }
}
