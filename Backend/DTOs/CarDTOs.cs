namespace Backend.DTOs
{
    public class CarDTOs
    {
        public int Id { get; set; }

        public int BrandID { get; set; }

        public string Model { get; set; } = null!;

        public int Year { get; set; }

        public decimal Milles { get; set; }

        public string? ImageUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Street { get; set; } = "";
        public string StreetNumber { get; set; } = "";
        public string Neighborhood { get; set; } = "";
        public string City { get; set; } = "";
    }
}
