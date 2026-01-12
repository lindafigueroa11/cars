using Microsoft.AspNetCore.Http;

namespace Backend.DTOs
{
    public class CarInsertDTOs
    {
        public int BrandID { get; set; }
        public int Milles { get; set; }
        public int Year { get; set; }
        public string Model { get; set; } = null!;

        // 🖼️ Imagen
        public IFormFile? Image { get; set; }

        // 📍 Ubicación (OBLIGATORIA)
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // 🏠 Dirección (opcional)
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
    }
}
