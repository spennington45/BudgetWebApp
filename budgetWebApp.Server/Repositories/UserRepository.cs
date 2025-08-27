using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User> AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByUserIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
