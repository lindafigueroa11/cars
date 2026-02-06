using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; } = null!;

        public int NumberCars { get; set; }

        public bool SaleCar { get; set; }

        public bool IsActive { get; set; } = true;

        public string? ProfileImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
