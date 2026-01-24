using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class CarService
    {
        private readonly StoreContext _context;
        private readonly ReverseGeocodingService _geo;

        public CarService(StoreContext context, ReverseGeocodingService geo)
        {
            _context = context;
            _geo = geo;
        }

        public async Task<IEnumerable<CarDTOs>> Get()
        {
            return await _context.Cars
                .Select(c => new CarDTOs
                {
                    Id = c.CarID,
                    BrandID = c.BrandID,
                    Model = c.Model,
                    Year = c.Year,
                    Price = c.Price,
                    Milles = c.Milles,
                    Color = c.Color,
                    IsAutomatic = c.IsAutomatic,
                    PublishedAt = c.PublishedAt,
                    ImageUrl = c.ImageUrl,
                    Latitude = c.CarLocation.Latitude,
                    Longitude = c.CarLocation.Longitude,
                    Street = c.CarLocation.Street,
                    City = c.CarLocation.City
                })
                .ToListAsync();
        }

        public async Task<CarDTOs?> GetById(int id)
        {
            return (await Get()).FirstOrDefault(c => c.Id == id);
        }

        public async Task<CarDTOs> Add(CarInsertDTOs dto)
        {
            var car = new Car
            {
                BrandID = dto.BrandID,
                Model = dto.Model,
                Year = dto.Year,
                Price = dto.Price,
                Milles = dto.Milles,
                Color = dto.Color,
                IsAutomatic = dto.IsAutomatic
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            var (street, city) = await _geo.GetAddressAsync(dto.Latitude, dto.Longitude);

            _context.CarLocations.Add(new CarLocation
            {
                CarID = car.CarID,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Street = street,
                City = city
            });

            await _context.SaveChangesAsync();

            return await GetById(car.CarID)
                   ?? throw new Exception("Error creando coche");
        }

        public async Task<int> DeleteAll()
        {
            _context.CarLocations.RemoveRange(_context.CarLocations);
            _context.Cars.RemoveRange(_context.Cars);
            await _context.SaveChangesAsync();
            return 1;
        }
    }
}
