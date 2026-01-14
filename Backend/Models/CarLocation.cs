using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class CarLocation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Car")]
        public int CarID { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Street { get; set; } = "";
        public string City { get; set; } = "";

        public Car Car { get; set; }
    }
}
