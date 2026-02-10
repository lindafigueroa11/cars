using Backend.DTOs.Auth;

namespace Backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginWithGoogle(LoginGoogleDTO dto);
    }
}
