using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class CarLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarLocationID { get; set; }

        [Required]
        public int CarID { get; set; }

        [ForeignKey("CarID")]
        public Car Car { get; set; } = null!;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Street { get; set; } = "";
        public string City { get; set; } = "";
    }
}
