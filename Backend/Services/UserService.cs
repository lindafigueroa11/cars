using Backend.DTOs.Users;
using Backend.Models;
using Backend.Repository.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserResponseDTO> Create(UserCreateDTO dto)
        {
            var user = new User
            {
                Name = dto.Name,
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                NumberCars = 0,
                SaleCar = false
            };

            await _repo.AddAsync(user);

            return Map(user);
        }

        public async Task<List<UserResponseDTO>> GetAll() =>
            (await _repo.GetAllAsync()).Select(Map).ToList();

        public async Task<UserResponseDTO?> GetById(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user == null ? null : Map(user);
        }

        public async Task Update(int id, UserUpdateDTO dto)
        {
            var user = await _repo.GetByIdAsync(id)
                ?? throw new Exception("User not found");

            user.Name = dto.Name;
            user.PhoneNumber = dto.PhoneNumber;
            user.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(user);
        }

        public async Task Delete(int id)
        {
            var user = await _repo.GetByIdAsync(id)
                ?? throw new Exception("User not found");

            await _repo.DeleteAsync(user);
        }

        private static UserResponseDTO Map(User u) => new()
        {
            Id = u.Id,
            Name = u.Name,
            UserName = u.UserName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            NumberCars = u.NumberCars,
            SaleCar = u.SaleCar,
            IsActive = u.IsActive
        };
    }
}
