using ObjectPoolSystem.Domain.Exceptions;
using ObjectPoolSystem.Domain.Interfaces;
using ObjectPoolSystem.Domain.Models;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace ObjectPoolSystem.Domain.Pools
{
    public class ObjectPool<T> where T : class, IPoolable
    {
        private readonly Func<T> _objectFactory;
        private readonly int _minSize;
        private readonly int _maxSize;
        private readonly ConcurrentBag<T> _availableObjects;
        private readonly SemaphoreSlim _semaphore;
        private readonly PoolStatistics _statistics;

        public PoolStatistics Statistics => _statistics;
        public ObjectPool(Func<T> objectFactory, int minSize, int maxSize, PoolStatistics statistics)
        {
            _objectFactory   = objectFactory;
            _minSize         = minSize;
            _maxSize         = maxSize;
            _statistics      = statistics;
            _availableObjects = new ConcurrentBag<T>();
            _semaphore       = new SemaphoreSlim(maxSize, maxSize);

            for (int i = 0; i < minSize; i++)
            {
                var obj = _objectFactory();
                _availableObjects.Add(obj);
            }
            _statistics.RecordInitialized(minSize);
        }

        public async Task<T> AcquireAsync(CancellationToken ct = default)
        {
            var sw = Stopwatch.StartNew();

            bool entered = await _semaphore.WaitAsync(TimeSpan.FromSeconds(30), ct);
            if (!entered)
                throw new PoolExhaustedException(
                    $"Pool exhausted: all {_maxSize} objects are in use. " +
                    "No slot freed within 30 seconds.");

            bool wasNewlyCreated = false;
            T item;

            if (_availableObjects.TryTake(out T? existing) && existing.IsValid())
            {
                item = existing;
            }
            else
            {
                item = _objectFactory();
                wasNewlyCreated = true;
            }

            item.LastUsedAt = DateTime.UtcNow;

            sw.Stop();
            _statistics.RecordAcquired(sw.Elapsed.TotalMilliseconds, wasNewlyCreated);

            return item;
        }

        public void Release(T item)
        {
            double usageDurationMs = (DateTime.UtcNow - item.LastUsedAt).TotalMilliseconds;

            bool returnedToPool = false;
            try
            {
                item.Reset();

                if (item.IsValid())
                {
                    _availableObjects.Add(item);
                    returnedToPool = true;
                }
                else
                {
                    item.Dispose();
                }
            }
            finally
            {
                _semaphore.Release();

                _statistics.RecordReleased(usageDurationMs, returnedToPool);
            }
        }
    }
}
