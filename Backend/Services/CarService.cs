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

        /* =======================
           GET ALL
        ======================= */
        public async Task<IEnumerable<CarDTOs>> Get()
        {
            return await (
                from car in _context.Cars
                join location in _context.CarLocations
                    on car.CarID equals location.CarID
                    into locations
                from location in locations.DefaultIfEmpty()
                select new CarDTOs
                {
                    Id = car.CarID,
                    BrandID = car.BrandID,
                    Model = car.Model,
                    Year = car.Year,
                    Price = car.Price,
                    Milles = car.Milles,
                    Color = car.Color,
                    IsAutomatic = car.IsAutomatic,
                    PublishedAt = car.PublishedAt,
                    ImageUrl = car.ImageUrl,

                    Latitude = location != null ? location.Latitude : 0,
                    Longitude = location != null ? location.Longitude : 0,
                    Street = location != null ? location.Street : "",
                    City = location != null ? location.City : ""
                }
            ).ToListAsync();
        }

        /* =======================
           GET BY ID
        ======================= */
        public async Task<CarDTOs?> GetById(int id)
        {
            return await (
                from car in _context.Cars
                join location in _context.CarLocations
                    on car.CarID equals location.CarID
                    into locations
                from location in locations.DefaultIfEmpty()
                where car.CarID == id
                select new CarDTOs
                {
                    Id = car.CarID,
                    BrandID = car.BrandID,
                    Model = car.Model,
                    Year = car.Year,
                    Price = car.Price,
                    Milles = car.Milles,
                    Color = car.Color,
                    IsAutomatic = car.IsAutomatic,
                    PublishedAt = car.PublishedAt,
                    ImageUrl = car.ImageUrl,

                    Latitude = location != null ? location.Latitude : 0,
                    Longitude = location != null ? location.Longitude : 0,
                    Street = location != null ? location.Street : "",
                    City = location != null ? location.City : ""
                }
            ).FirstOrDefaultAsync();
        }

        /* =======================
           CREATE
        ======================= */
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
                IsAutomatic = dto.IsAutomatic,
                PublishedAt = DateTime.UtcNow
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            var (street, city) =
                await _geo.GetAddressAsync(dto.Latitude, dto.Longitude);

            var location = new CarLocation
            {
                CarID = car.CarID,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Street = street,
                City = city
            };

            _context.CarLocations.Add(location);
            await _context.SaveChangesAsync();

            return await GetById(car.CarID)
                   ?? throw new Exception("Error creando coche");
        }

        /* =======================
           DELETE ALL
        ======================= */
        public async Task<int> DeleteAll()
        {
            var locations = await _context.CarLocations.ToListAsync();
            _context.CarLocations.RemoveRange(locations);

            var cars = await _context.Cars.ToListAsync();
            _context.Cars.RemoveRange(cars);

            await _context.SaveChangesAsync();
            return cars.Count;
        }
    }
}
