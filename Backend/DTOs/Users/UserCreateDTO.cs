namespace Backend.DTOs.Users
{
    public class UserCreateDTO
    {
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public bool SaleCar { get; set; }
        public bool IsActive { get; set; }
    }
}