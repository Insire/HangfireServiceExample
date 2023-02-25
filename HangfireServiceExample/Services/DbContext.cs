using Microsoft.Extensions.Logging;

namespace HangfireServiceExample.Services
{
    public sealed class DbContext : IDbContext
    {
        private readonly ILogger<DbContext> _logger;

        public Guid Guid { get; }

        public DbContext(ILogger<DbContext> logger)
        {
            _logger = logger;
            Guid = Guid.NewGuid();
            _logger.LogInformation("{ID} DbContext Created", Guid);
        }

        public Task SaveChangesAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            _logger.LogInformation("{ID} SaveChangesAsync", Guid);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("{ID} DbContext Disposed", Guid);
        }
    }
}
