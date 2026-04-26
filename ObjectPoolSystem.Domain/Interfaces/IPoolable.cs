using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPoolSystem.Domain.Interfaces
{
    public interface IPoolable : IDisposable
    {
        bool IsValid();
        void Reset();
        DateTime CreatedAt { get; }
        DateTime LastUsedAt { get; set; }
    }
}
