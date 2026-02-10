namespace Backend.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}

