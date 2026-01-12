using Backend.DTOs;
using Backend.Models;
using Backend.Repository;

namespace Backend.Services
{
    public class CarLocationService
    {
        private readonly CarLocationRepository _repository;

        public CarLocationService(CarLocationRepository repository)
        {
            _repository = repository;
        }

        // Guardar ubicación del coche
        public async Task Add(CarLocationInsertDTOs dto)
        {
            var location = new CarLocation
            {
                CarID = dto.CarID,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Street = dto.Street,
                StreetNumber = dto.StreetNumber,
                Neighborhood = dto.Neighborhood,
                City = dto.City
            };

            await _repository.Add(location);
            await _repository.Save();
        }

        // Obtener ubicaciones para el mapa
        public async Task<IEnumerable<CarLocationMapDTOs>> GetForMap()
        {
            var locations = await _repository.GetAll();

            return locations.Select(l => new CarLocationMapDTOs
            {
                CarID = l.CarID,
                Latitude = l.Latitude,
                Longitude = l.Longitude
            });
        }
    }
}
