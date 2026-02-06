using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {
        }

        // =======================
        // DbSets
        // =======================
        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CarLocation> CarLocations { get; set; }

        // =======================
        // Model configuration
        // =======================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔗 User (1) -> Cars (many)
            modelBuilder.Entity<Car>()
                .HasOne(c => c.User)          // Car tiene un User
                .WithMany(u => u.Cars)        // User tiene muchos Cars
                .HasForeignKey(c => c.UserId) // FK en Cars
                .OnDelete(DeleteBehavior.Cascade);

            // Opcional: índices útiles
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();
        }
    }
}
