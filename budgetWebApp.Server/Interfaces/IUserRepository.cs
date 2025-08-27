using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserIdAsync(long id);

        Task<User> UpdateUserAsync(User user);

        Task<User> AddUserAsync(User user);
    }
}
