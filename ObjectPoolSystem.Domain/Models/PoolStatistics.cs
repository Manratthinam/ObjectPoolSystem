using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPoolSystem.Domain.Models
{
    public class PoolStatistics
    {
        private readonly object _lock = new();
        public int TotalCreated { get; private set; }
        public int AvailableCount { get; private set; }
        public int InUseCount { get; private set; }

        public int PeakInUse { get; private set; }

        private int _acquireSamples;
        private int _usageSamples;

        public void RecordInitialized(int count)
        {
            lock (_lock)
            {
                TotalCreated += count;
                AvailableCount += count;
            }
        }

        public void RecordAcquired(double acquireTimeMs, bool wasNewlyCreated)
        {
            lock (_lock)
            {
                if (wasNewlyCreated)
                    TotalCreated++;
                else
                    AvailableCount--; // Only decrement if it came FROM the available bag

                InUseCount++;

                if (InUseCount > PeakInUse)
                    PeakInUse = InUseCount;

                _acquireSamples++;
            }
        }

        public void RecordReleased(double usageDurationMs, bool returnedToPool)
        {
            lock (_lock)
            {
                InUseCount = InUseCount - 1;

                if (returnedToPool)
                    AvailableCount++;

                _usageSamples++;
            }
        }

        public override string ToString() =>
            $"Available={AvailableCount}, InUse={InUseCount} " +
                $"TotalCreated={TotalCreated}, ";
    }
}
