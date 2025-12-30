using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace budgetWebApp.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BudgetContext _context;

        public UserRepository(BudgetContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            var newUser = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return newUser.Entity;
        }

        public Task<User> GetUserByUserEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<User> GetUserByUserIdAsync(long id)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var updatedUser = _context.Update(user);
            await _context.SaveChangesAsync();
            return updatedUser.Entity;
        }
    }
}
