using Backend.DTOs.Users;

namespace Backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> Create(UserCreateDTO dto);
        Task<List<UserResponseDTO>> GetAll();
        Task<UserResponseDTO?> GetById(int id);
        Task Update(int id, UserUpdateDTO dto);
        Task Delete(int id);
    }
}
