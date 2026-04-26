using System.Threading.Tasks;

namespace ObjectPoolSystem.Application.Interface
{
    public interface IUserRepository
    {
        Task<int> GetTotalUsersAsync();
    }
}
