using Backend.DTOs;
using Backend.Models;
using Backend.Repository;

namespace Backend.Services
{
    public class CarLocationService
    {
        private readonly IRepository<CarLocation> _repository;

        public CarLocationService(IRepository<CarLocation> repository)
        {
            _repository = repository;
        }

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

            await _repository.Add(location);
            await _repository.Save();
        }

        public async Task<IEnumerable<CarLocationMapDTOs>> GetForMap()
        {
            var locations = await _repository.Get();

            return locations.Select(l => new CarLocationMapDTOs
            {
                CarID = l.CarID,
                Latitude = l.Latitude,
                Longitude = l.Longitude
            });
        }
    }
}
