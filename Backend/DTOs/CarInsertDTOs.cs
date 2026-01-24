using Microsoft.AspNetCore.Http;

namespace Backend.DTOs
{
    public class CarInsertDTOs
    {
        public int BrandID { get; set; }
        public string Model { get; set; } = "";
        public int Year { get; set; }

        public decimal Price { get; set; }
        public decimal Milles { get; set; }

        public string Color { get; set; } = "";
        public bool IsAutomatic { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? Street { get; set; }
        public string? City { get; set; }

        public IFormFile? Image { get; set; }
    }
}
