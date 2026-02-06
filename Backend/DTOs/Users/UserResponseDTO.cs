namespace Backend.DTOs.Users
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int NumberCars { get; set; }
        public string PhoneNumber { get; set; }
        public bool SaleCar { get; set; }
        public bool IsActive { get; set; }
    }

}
