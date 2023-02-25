using System.Diagnostics.Tracing;

namespace HangfireServiceExample.Impl.Metrics
{
    [EventSource(Name = SourceName)]
    public class HangfireCountersEventSource : EventSource
    {
        // this name will be used as "provider" name with dotnet-counters
        // ex: dotnet-counters monitor -p <pid> Hangfire.Counters
        private const string SourceName = "Hangfire.Counters";

        private long _created;
        private long _performed;
        private long _performedWithExceptions;

        private long _succeeded;
        private long _deleted;
        private long _failed;

        public HangfireCountersEventSource()
            : base(SourceName, EventSourceSettings.EtwSelfDescribingEventFormat)
        {
            CreatedPerSec = new IncrementingPollingCounter("created-rate", this, () => _created)
            {
                DisplayName = "Background Jobs Created",
                DisplayRateTimeScale = new TimeSpan(0, 0, 1)
            };

            Created = new PollingCounter("created", this, () => _created)
            {
                DisplayName = "Background Jobs Created"
            };

            PerformedPerSec = new IncrementingPollingCounter("performed-rate", this, () => _performed)
            {
                DisplayName = "Background Jobs Performed",
                DisplayRateTimeScale = new TimeSpan(0, 0, 1)
            };

            Performed = new PollingCounter("performed", this, () => _performed)
            {
                DisplayName = "Background Jobs Performed"
            };

            PerformedWithExceptionsPerSec = new IncrementingPollingCounter("performed-exceptions-rate", this, () => _performedWithExceptions)
            {
                DisplayName = "Background Jobs Performed with Exceptions",
                DisplayRateTimeScale = new TimeSpan(0, 0, 1)
            };

            PerformedWithExceptions = new PollingCounter("performed-exceptions", this, () => _performedWithExceptions)
            {
                DisplayName = "Background Jobs Performed with Exceptions"
            };

            SucceededPerSec = new IncrementingPollingCounter("succeeded-rate", this, () => _succeeded)
            {
                DisplayName = "Transitions to Succeeded State",
                DisplayRateTimeScale = new TimeSpan(0, 0, 1)
            };

            Succeeded = new PollingCounter("succeeded", this, () => _succeeded)
            {
                DisplayName = "Transitions to Succeeded State"
            };

            DeletedPerSec = new IncrementingPollingCounter("deleted-rate", this, () => _deleted)
            {
                DisplayName = "Transitions to Deleted State",
                DisplayRateTimeScale = new TimeSpan(0, 0, 1)
            };

            Deleted = new PollingCounter("deleted", this, () => _deleted)
            {
                DisplayName = "Transitions to Deleted State"
            };

            FailedPerSec = new IncrementingPollingCounter("failed-rate", this, () => _failed)
            {
                DisplayName = "Transitions to Failed State",
                DisplayRateTimeScale = new TimeSpan(0, 0, 1)
            };

            Failed = new PollingCounter("failed", this, () => _failed)
            {
                DisplayName = "Transitions to Failed State"
            };
        }

        public IncrementingPollingCounter CreatedPerSec { get; private set; }
        public PollingCounter Created { get; private set; }

        public IncrementingPollingCounter PerformedPerSec { get; private set; }
        public PollingCounter Performed { get; private set; }

        public IncrementingPollingCounter PerformedWithExceptionsPerSec { get; private set; }
        public PollingCounter PerformedWithExceptions { get; private set; }

        public IncrementingPollingCounter SucceededPerSec { get; private set; }
        public PollingCounter Succeeded { get; private set; }

        public IncrementingPollingCounter DeletedPerSec { get; private set; }
        public PollingCounter Deleted { get; private set; }

        public IncrementingPollingCounter FailedPerSec { get; private set; }
        public PollingCounter Failed { get; private set; }

        public void IncrementCreated()
        {
            Interlocked.Increment(ref _created);
        }

        public void IncrementPerformed()
        {
            Interlocked.Increment(ref _performed);
        }

        public void IncrementPerformedWithExceptions()
        {
            Interlocked.Increment(ref _performedWithExceptions);
        }

        public void IncrementSucceeded()
        {
            Interlocked.Increment(ref _succeeded);
        }

        public void IncrementDeleted()
        {
            Interlocked.Increment(ref _deleted);
        }

        public void IncrementFailed()
        {
            Interlocked.Increment(ref _failed);
        }
    }
}
