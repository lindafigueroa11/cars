namespace Backend.DTOs
{
    public class CarUpdateDTOs
    {
        public int Id { get; set; }
        public decimal Milles { get; set; }
        public int Year { get; set; }
        public int BrandID { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }

        // 📍 Ubicación
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? Street { get; set; }
        public string? City { get; set; }

        // 🖼️ Imagen opcional
        public IFormFile? Image { get; set; }

    }
}
