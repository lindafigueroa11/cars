using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CarLocation> CarLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Car -> CarLocation (1 a 1)
            modelBuilder.Entity<CarLocation>()
                .HasOne(cl => cl.Car)
                .WithOne()
                .HasForeignKey<CarLocation>(cl => cl.CarID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
