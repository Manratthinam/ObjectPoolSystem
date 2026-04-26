using ObjectPoolSystem.Application.Interface;
using ObjectPoolSystem.Domain.Interfaces;
using ObjectPoolSystem.Domain.Pools;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectPoolSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ObjectPool<IPoolDbConnection> _pool;

        public UserRepository(ObjectPool<IPoolDbConnection> pool)
        {
            _pool = pool;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            var connection = await _pool.AcquireAsync(CancellationToken.None);
            try
            {
                return await connection.GetTotalUsersAsync();
            }
            finally
            {
                _pool.Release(connection);
            }
        }
    }
}
