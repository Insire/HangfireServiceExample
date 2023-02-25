namespace HangfireServiceExample.Services
{
    public interface IDbContext : IDisposable
    {
        Guid Guid { get; }

        Task SaveChangesAsync(CancellationToken token);
    }
}
