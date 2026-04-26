using Microsoft.AspNetCore.Mvc;
using ObjectPoolSystem.Domain.Pools;
using ObjectPoolSystem.Infrastructure.Resources;

using ObjectPoolSystem.Domain.Interfaces;

namespace ObjectPoolSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemController : ControllerBase
    {
        private readonly ObjectPool<IPoolDbConnection> _pool;
        private readonly ObjectPool<IPoolSmtpConnection> _poolSmtp;

        public SystemController(ObjectPool<IPoolDbConnection> pool, ObjectPool<IPoolSmtpConnection> poolSmtp)
        {
            _pool = pool;
            _poolSmtp = poolSmtp;

        }

        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var stat = new
            {
                DB = _pool.Statistics,
                Smtp = _poolSmtp.Statistics
            };
            return Ok(stat);
        }
    }
}
