using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options) { }

        public DbSet<Car> Cars => Set<Car>();
        public DbSet<CarLocation> CarLocations => Set<CarLocation>();
    }
}
