using Hangfire.Client;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

namespace HangfireServiceExample.Impl.Metrics
{
    public class HangfireCountersFilter : IApplyStateFilter, IClientFilter, IServerFilter
    {
        private readonly HangfireCountersEventSource _counters;

        public HangfireCountersFilter()
        {
            _counters = new HangfireCountersEventSource();
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            if (context.NewState is FailedState)
            {
                _counters.IncrementFailed();
            }

            if (context.NewState is SucceededState)
            {
                _counters.IncrementSucceeded();
            }

            if (context.NewState is DeletedState)
            {
                _counters.IncrementDeleted();
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }

        public void OnCreating(CreatingContext filterContext)
        {
        }

        public void OnCreated(CreatedContext filterContext)
        {
            _counters.IncrementCreated();
        }

        public void OnPerforming(PerformingContext filterContext)
        {
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            _counters.IncrementPerformed();

            if (filterContext.Exception != null && !filterContext.ExceptionHandled)
            {
                _counters.IncrementPerformedWithExceptions();
            }
        }
    }
}
