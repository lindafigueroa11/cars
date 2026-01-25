using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class CarLocationService
    {
        private readonly StoreContext _context;

        public CarLocationService(StoreContext context)
        {
            _context = context;
        }

        /* =======================
           CREATE
        ======================= */
        public async Task Add(CarLocationInsertDTOs dto)
        {
            var location = new CarLocation
            {
                CarID = dto.CarID,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Street = dto.Street ?? "",
                City = dto.City ?? ""
            };

            _context.CarLocations.Add(location);
            await _context.SaveChangesAsync();
        }

        /* =======================
           GET FOR MAP
        ======================= */
        public async Task<IEnumerable<CarLocationMapDTOs>> GetForMap()
        {
            return await _context.CarLocations
                .Select(l => new CarLocationMapDTOs
                {
                    CarID = l.CarID,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude
                })
                .ToListAsync();
        }
    }
}
