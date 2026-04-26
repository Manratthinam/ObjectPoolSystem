using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ObjectPoolSystem.Domain.Interfaces;
using ObjectPoolSystem.Domain.Pools;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectPoolSystem.Application.Services
{
    public class PoolMonitorService : BackgroundService
    {
        private readonly ObjectPool<IPoolDbConnection> _dbPool;
        private readonly ObjectPool<IPoolSmtpConnection> _smtpPool;
        private readonly ILogger<PoolMonitorService> _logger;

        public PoolMonitorService(
            ObjectPool<IPoolDbConnection> dbPool,
            ObjectPool<IPoolSmtpConnection> smtpPool,
            ILogger<PoolMonitorService> logger)
        {
            _dbPool = dbPool;
            _smtpPool = smtpPool;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var dbStats   = _dbPool.Statistics;
                var smtpStats = _smtpPool.Statistics;

                _logger.LogInformation(
                    "[DB Pool]   Available={Available}, InUse={InUse}, TotalCreated={Total}",
                    dbStats.AvailableCount, dbStats.InUseCount, dbStats.TotalCreated);

                _logger.LogInformation(
                    "[SMTP Pool] Available={Available}, InUse={InUse}, TotalCreated={Total}",
                    smtpStats.AvailableCount, smtpStats.InUseCount, smtpStats.TotalCreated);

                await Task.Delay(15000, stoppingToken);
            }
        }
    }
}
