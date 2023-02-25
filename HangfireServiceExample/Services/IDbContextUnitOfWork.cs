namespace HangfireServiceExample.Services
{
    public interface IDbContextUnitOfWork : IAsyncDisposable
    {
        IDbContext Create();
    }
}
