using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class CarRepository : IRepository<Car>
    {
        private StoreContext _context;
        public CarRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Car>> Get() => await _context.Cars.ToListAsync();

        public async Task<Car> GetById(int Id)
        => await _context.Cars.FindAsync(Id);
        public async Task Add(Car car)
        =>    await _context.Cars.AddAsync(car);

        public async void Update(Car car)
        {
            _context.Cars.Attach(car);
            _context.Cars.Entry(car).State = EntityState.Modified;
        }

        public void Delete(Car car)
         => _context.Cars.Remove(car);
        public async Task Save()
            => await _context.SaveChangesAsync();
    }
}
