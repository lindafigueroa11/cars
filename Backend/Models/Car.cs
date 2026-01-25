using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarID { get; set; }

        public string Model { get; set; } = "";
        public int BrandID { get; set; }
        public int Year { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,1)")]
        public decimal Milles { get; set; }

        public bool IsAutomatic { get; set; }
        public string Color { get; set; } = "";

        public DateTime? PublishedAt { get; set; }

        public string? ImageUrl { get; set; }
    }
}
