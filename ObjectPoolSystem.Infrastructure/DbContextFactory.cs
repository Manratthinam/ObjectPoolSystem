using ObjectPoolSystem.Application;
using ObjectPoolSystem.Infrastructure.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPoolSystem.Infrastructure
{
    public class DbContextFactory : IResourceFactory<PoolDbContext>
    {
        public readonly string _connectionString;

        public DbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public PoolDbContext create()
        {
            return new PoolDbContext(_connectionString);
        }
    }
}
