namespace Backend.DTOs.Users
{
    public class UserUpdateDTO
    {
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsActive { get; set; }
    }

}
