using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPoolSystem.Domain.Interfaces
{
    public interface IPoolDbConnection : IPoolable
    {
        Task<int> GetTotalUsersAsync();
    }
}
