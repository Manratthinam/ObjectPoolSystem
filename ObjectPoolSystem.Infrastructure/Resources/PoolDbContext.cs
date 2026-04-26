using Microsoft.EntityFrameworkCore;
using ObjectPoolSystem.Domain.Interfaces;
using ObjectPoolSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPoolSystem.Infrastructure.Resources
{
    public class PoolDbContext : IPoolDbConnection
    {
        private AppDBContext _context;

        public PoolDbContext(string connection)
        {
            _context = new AppDBContext(new DbContextOptionsBuilder<AppDBContext>().UseNpgsql(connection).Options);
        }
        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        public DateTime LastUsedAt { get; set; } = DateTime.UtcNow;

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public bool IsValid()
        {
            return  _context.Database.CanConnect();
        }

        public void Reset()
        {
            _context.ChangeTracker.Clear();
        }

        public void EnsureCreated()
        {
            _context.Database.EnsureCreated();
        }
    }
}
