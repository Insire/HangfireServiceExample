using Microsoft.Extensions.Logging;

namespace HangfireServiceExample.Services
{
    public sealed class DbContextFactory : IDbContextFactory
    {
        private readonly ILogger<DbContextFactory> _logger;
        private readonly ILoggerFactory _loggerFactory;

        private IDbContext? _dbContext;

        public DbContextFactory(ILogger<DbContextFactory> logger, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

        public IDbContext Create()
        {
            using (_logger.BeginScope("DbContextScope"))
            {
                return _dbContext ??= new DbContext(_loggerFactory.CreateLogger<DbContext>());
            }
        }

        public void Dispose()
        {
            var dbContext = _dbContext;
            if (dbContext == null)
            {
                return;
            }

            _logger.LogInformation("{ID} DbContextFactory Disposed", dbContext.Guid);
        }
    }
}
