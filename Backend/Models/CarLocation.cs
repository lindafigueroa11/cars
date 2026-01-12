namespace Backend.Models
{
    public class CarLocation
    {
        public int Id { get; set; }

        public int CarID { get; set; }
        public Car Car { get; set; } = null!;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Street { get; set; } = null!;
        public string StreetNumber { get; set; } = null!;
        public string Neighborhood { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
