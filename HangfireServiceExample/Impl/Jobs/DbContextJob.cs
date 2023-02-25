using HangfireServiceExample.Services;

namespace HangfireServiceExample.Impl.Jobs
{
    [RegisterScoped]
    [HangfireJobName("DbContext")]
    public sealed class DbContextJob : IDbContextJob
    {
        private readonly IDbContext _dbContext;

        public DbContextJob(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Run(CancellationToken token)
        {
            await _dbContext.SaveChangesAsync(token);
        }
    }
}
