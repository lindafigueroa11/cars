using Backend.Models;
using Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly StoreContext _context;

        public UserRepository(StoreContext context)
        {
            _context = context;
        }

        public Task<User?> GetByEmailAsync(string email) =>
            _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public Task<User?> GetByIdAsync(int id) =>
            _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
