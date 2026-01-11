namespace Backend.DTOs
{
    public class CarDTOs
    {
        public int Id { get; set; }
        public decimal Milles { get; set; }
        public int Year { get; set; }
        public int BrandID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public IFormFile? Image { get; set; }
    }
}
