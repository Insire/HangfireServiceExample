namespace HangfireServiceExample.Services
{
    public interface IDbContextFactory : IDisposable
    {
        IDbContext Create();
    }
}
