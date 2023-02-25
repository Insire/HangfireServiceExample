using Microsoft.Extensions.Logging;

namespace HangfireServiceExample.Services
{
    public sealed class DbContextUnitOfWork : IDbContextUnitOfWork
    {
        private readonly IDbContextFactory _factory;
        private readonly ILogger<DbContextUnitOfWork> _logger;
        private IDbContext? _dbContext;

        public DbContextUnitOfWork(IDbContextFactory factory, ILogger<DbContextUnitOfWork> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public IDbContext Create()
        {
            return _dbContext ??= _factory.Create();
        }

        public async ValueTask DisposeAsync()
        {
            var dbContext = _dbContext;
            if (dbContext == null)
            {
                return;
            }

            await dbContext.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false);

            _logger.LogInformation("{ID} DbContextUnitOfWork Disposed", dbContext.Guid);
        }
    }
}
