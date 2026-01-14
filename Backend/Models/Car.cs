using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarID { get; set; }

        [Required]
        public string Model { get; set; } = null!;

        public int BrandID { get; set; }
        public int Year { get; set; }

        [Column(TypeName = "decimal(18,1)")]
        public decimal Milles { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [ForeignKey("BrandID")]
        public virtual Brand Brand { get; set; } = null!;

        public virtual CarLocation? Location { get; set; }
    }
}
