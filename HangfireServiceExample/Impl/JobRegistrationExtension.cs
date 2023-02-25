using Hangfire;
using Hangfire.Common;
using HangfireServiceExample.Impl.Jobs;
using System.Reflection;

namespace HangfireServiceExample.Impl
{
    public static class JobRegistrationExtension
    {
        public static void RegisterJobs(this IRecurringJobManager jobManager)
        {
            var jobType = typeof(IJob);
            foreach (var type in Assembly.GetAssembly(typeof(Program))!.GetTypes().Where(p => jobType.IsAssignableFrom(p) && p.IsClass))
            {
                var nameAttribute = type.GetCustomAttribute<HangfireJobNameAttribute>() ?? throw new InvalidOperationException();
                var method = type.GetMethod(nameof(IJob.Run)) ?? throw new InvalidOperationException();

                var job = new Job(type, method, CancellationToken.None);

                jobManager.AddOrUpdate(nameAttribute.Name, job, "* * * * *");
            }
        }
    }
}
