using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class CarRepository : IRepository<Car>
    {
        private readonly StoreContext _context;

        public CarRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> Get()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car?> GetById(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task Add(Car car)
        {
            await _context.Cars.AddAsync(car);
        }

        public void Update(Car car)
        {
            _context.Cars.Update(car);
        }

        public void Delete(Car car)
        {
            _context.Cars.Remove(car);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
