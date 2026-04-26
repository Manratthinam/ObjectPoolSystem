using ObjectPoolSystem.Application.Interface;
using ObjectPoolSystem.Domain.Interfaces;
using ObjectPoolSystem.Domain.Pools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPoolSystem.Application.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IUserRepository _userRepository;

        public DatabaseService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<int> ExecuteQueryAsync()
        {
            return _userRepository.GetTotalUsersAsync();
        }

    }
}
