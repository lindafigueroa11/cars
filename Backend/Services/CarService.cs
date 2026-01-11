using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Backend.Services
{
    public class CarService : ICommonService<CarDTOs, CarInsertDTOs, CarUpdateDTOs>
    {
        private readonly IRepository<Car> _repository;
        private readonly Cloudinary _cloudinary;

        public CarService(
            IRepository<Car> repository,
            Cloudinary cloudinary)
        {
            _repository = repository;
            _cloudinary = cloudinary;
        }

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

        public async Task<CarDTOs> Add(CarInsertDTOs dto)
        {
            string? imageUrl = null;

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

            var car = new Car
            {
                BrandID = dto.BrandID,
                Milles = dto.Milles,
                Model = dto.Model,
                Year = dto.Year,
                ImageUrl = imageUrl
            };

            await _repository.Add(car);
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

        public async Task<CarDTOs?> Update(int id, CarUpdateDTOs dto)
        {
            var car = await _repository.GetById(id);
            if (car == null) return null;

            car.BrandID = dto.BrandID;
            car.Milles = dto.Milles;
            car.Model = dto.Model;
            car.Year = dto.Year;

            _repository.Update(car);
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
