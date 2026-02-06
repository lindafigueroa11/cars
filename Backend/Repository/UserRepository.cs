using Backend.Models;
using Backend.Repository.Interface;
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

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<User?> GetByIdAsync(int id) =>
            _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        public Task<List<User>> GetAllAsync() =>
            _context.Users.ToListAsync();

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

}
