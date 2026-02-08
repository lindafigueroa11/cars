using Backend.DTOs.Users;

namespace Backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> Login(LoginDTO dto);
    }
}
