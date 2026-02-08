namespace Backend.DTOs.Users
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
