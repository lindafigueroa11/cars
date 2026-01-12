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

        public CarService(
            IRepository<Car> repository,
            Cloudinary cloudinary,
            StoreContext context
        )
        {
            _repository = repository;
            _cloudinary = cloudinary;
            _context = context;
        }

        /* =======================
           GET ALL
        ======================= */
        public async Task<IEnumerable<CarDTOs>> Get()
        {
            var cars = await _repository.Get();

            return cars.Select(car => new CarDTOs
            {
                Id = car.CarID,
                BrandID = car.BrandID,
                Milles = car.Milles,
                Model = car.Model,
                Year = car.Year,
                ImageUrl = car.ImageUrl
            });
        }

        /* =======================
           GET BY ID
        ======================= */
        public async Task<CarDTOs?> GetById(int id)
        {
            var car = await _repository.GetById(id);
            if (car == null) return null;

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

            // 📍 Crear ubicación
            var location = new CarLocation
            {
                CarID = car.CarID,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Street = dto.Street ?? "",
                StreetNumber = dto.StreetNumber ?? "",
                Neighborhood = dto.Neighborhood ?? "",
                City = dto.City ?? ""
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

            // 🖼️ Actualizar imagen (opcional)
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
                location = new CarLocation
                {
                    CarID = car.CarID
                };
                _context.CarLocations.Add(location);
            }

            location.Latitude = dto.Latitude;
            location.Longitude = dto.Longitude;
            location.Street = dto.Street ?? "";
            location.StreetNumber = dto.StreetNumber ?? "";
            location.Neighborhood = dto.Neighborhood ?? "";
            location.City = dto.City ?? "";

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
