namespace Backend.DTOs
{
    public class CarDTOs
    {
        public int Id { get; set; }
        public int BrandID { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Milles { get; set; }
        public decimal? Price { get; set; }
        public string? Color { get; set; }
        public bool? IsAutomatic { get; set; }
        public DateTime PublishedAt { get; set; }
        public string? ImageUrl { get; set; }


        // 📍 UBICACIÓN (LO QUE FALTABA)
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? Street { get; set; }
        public string? City { get; set; }
    }
}
