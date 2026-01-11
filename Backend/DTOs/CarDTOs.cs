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
    }
}
