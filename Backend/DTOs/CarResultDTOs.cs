namespace Backend.DTOs
{
    public class CarResultDTO
    {
        public int CarID { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public decimal? Milles { get; set; }
        public decimal? Price { get; set; }
        public bool? IsAutomatic { get; set; }
        public string? ImageUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
    }
}