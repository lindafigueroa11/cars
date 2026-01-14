using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class CarService : ICommonService<CarDTOs, CarInsertDTOs, CarUpdateDTOs>
    {
        private readonly IRepository<Car> _repository;
        private readonly Cloudinary _cloudinary;
        private readonly StoreContext _context;
        private readonly ReverseGeocodingService _geoService;

        public CarService(
            IRepository<Car> repository,
            Cloudinary cloudinary,
            StoreContext context,
            ReverseGeocodingService geoService
        )
        {
            _repository = repository;
            _cloudinary = cloudinary;
            _context = context;
            _geoService = geoService;
        }

        /* =======================
           GET ALL
        ======================= */
        public async Task<IEnumerable<CarDTOs>> Get()
        {
            var cars = await (
                from car in _context.Cars
                join location in _context.CarLocations
                    on car.CarID equals location.CarID
                select new CarDTOs
                {
                    Id = car.CarID,
                    BrandID = car.BrandID,
                    Milles = car.Milles,
                    Model = car.Model,
                    Year = car.Year,
                    ImageUrl = car.ImageUrl,

                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Street = location.Street,
                    City = location.City
                }
            ).ToListAsync();

            return cars;
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
                where car.CarID == id
                select new CarDTOs
                {
                    Id = car.CarID,
                    BrandID = car.BrandID,
                    Milles = car.Milles,
                    Model = car.Model,
                    Year = car.Year,
                    ImageUrl = car.ImageUrl,

                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Street = location.Street,
                    City = location.City
                }
            ).FirstOrDefaultAsync();
        }


        /* =======================
           CREATE
        ======================= */
        public async Task<CarDTOs> Add(CarInsertDTOs dto)
        {
            string? imageUrl = null;

            // 🖼️ Subir imagen
            if (dto.Image != null && dto.Image.Length > 0)
            {
                if (!dto.Image.ContentType.StartsWith("image/"))
                    throw new Exception("El archivo no es una imagen");

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(
                        dto.Image.FileName,
                        dto.Image.OpenReadStream()
                    ),
                    Folder = "cars"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                imageUrl = uploadResult.SecureUrl.ToString();
            }

            // 🚗 Crear coche
            var car = new Car
            {
                BrandID = dto.BrandID,
                Milles = dto.Milles,
                Model = dto.Model,
                Year = dto.Year,
                ImageUrl = imageUrl
            };

            await _repository.Add(car);
            await _repository.Save(); // obtiene CarID

            // 📍 Autocompletar dirección si no viene
            var (street, number, neighborhood, city) =
                await _geoService.GetAddressAsync(dto.Latitude, dto.Longitude);

            // 📍 Crear ubicación
            var location = new CarLocation
            {
                CarID = car.CarID,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Street = string.IsNullOrEmpty(dto.Street) ? street : dto.Street,
                City = string.IsNullOrEmpty(dto.City) ? city : dto.City
            };

            _context.CarLocations.Add(location);
            await _context.SaveChangesAsync();

            return new CarDTOs
            {
                Id = car.CarID,
                BrandID = car.BrandID,
                Milles = car.Milles,
                Model = car.Model,
                Year = car.Year,
                ImageUrl = car.ImageUrl
            };
        }

        /* =======================
           UPDATE
        ======================= */
        public async Task<CarDTOs?> Update(int id, CarUpdateDTOs dto)
        {
            var car = await _repository.GetById(id);
            if (car == null) return null;

            // 🖼️ Imagen opcional
            if (dto.Image != null && dto.Image.Length > 0)
            {
                if (!dto.Image.ContentType.StartsWith("image/"))
                    throw new Exception("El archivo no es una imagen");

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(
                        dto.Image.FileName,
                        dto.Image.OpenReadStream()
                    ),
                    Folder = "cars"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                car.ImageUrl = uploadResult.SecureUrl.ToString();
            }

            // 🚗 Datos del coche
            car.BrandID = dto.BrandID;
            car.Milles = dto.Milles;
            car.Model = dto.Model;
            car.Year = dto.Year;

            _repository.Update(car);
            await _repository.Save();

            // 📍 Ubicación
            var location = await _context.CarLocations
                .FirstOrDefaultAsync(l => l.CarID == car.CarID);

            if (location == null)
            {
                location = new CarLocation { CarID = car.CarID };
                _context.CarLocations.Add(location);
            }

            // Autocompletar si no viene
            var (street, number, neighborhood, city) =
                await _geoService.GetAddressAsync(dto.Latitude, dto.Longitude);

            location.Latitude = dto.Latitude;
            location.Longitude = dto.Longitude;
            location.Street = string.IsNullOrEmpty(dto.Street) ? street : dto.Street;
            location.City = string.IsNullOrEmpty(dto.City) ? city : dto.City;

            await _context.SaveChangesAsync();

            return new CarDTOs
            {
                Id = car.CarID,
                BrandID = car.BrandID,
                Milles = car.Milles,
                Model = car.Model,
                Year = car.Year,
                ImageUrl = car.ImageUrl
            };
        }

        /* =======================
           DELETE
        ======================= */
        public async Task<CarDTOs?> Delete(int id)
        {
            var car = await _repository.GetById(id);
            if (car == null) return null;

            _repository.Delete(car);
            await _repository.Save();

            return new CarDTOs
            {
                Id = car.CarID,
                BrandID = car.BrandID,
                Milles = car.Milles,
                Model = car.Model,
                Year = car.Year,
                ImageUrl = car.ImageUrl
            };
        }
    }
}
