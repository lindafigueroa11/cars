using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class CarLocationRepository
    {
        private readonly StoreContext _context;
        public CarLocationRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(CarLocation location)
        {
            await _context.CarLocations.AddAsync(location);
        }
        public async Task<List<CarLocation>> GetAll()
        {
            return await _context.CarLocations.ToListAsync();
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}