using Microsoft.Extensions.Logging;

namespace HangfireServiceExample.Impl.Jobs
{
    [RegisterScoped]
    [HangfireJobName("Test")]
    public sealed class TestJob : ITestJob
    {
        private readonly ILogger<TestJob> logger;

        public TestJob(ILogger<TestJob> logger)
        {
            this.logger = logger;
        }

        public Task Run(CancellationToken token)
        {
            logger.LogInformation("{Name} has run", nameof(TestJob));

            return Task.CompletedTask;
        }
    }
}
