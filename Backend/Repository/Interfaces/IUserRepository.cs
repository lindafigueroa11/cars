using Backend.Models;

namespace Backend.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
    }
}
