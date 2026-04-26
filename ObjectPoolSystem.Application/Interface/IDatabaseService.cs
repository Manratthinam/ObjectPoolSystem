using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPoolSystem.Application.Interface
{
    public interface IDatabaseService
    {
        Task<int> ExecuteQueryAsync();
    }
}
