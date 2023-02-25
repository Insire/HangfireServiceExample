namespace HangfireServiceExample.Impl.Jobs
{
    public interface IJob
    {
        Task Run(CancellationToken token);
    }
}
