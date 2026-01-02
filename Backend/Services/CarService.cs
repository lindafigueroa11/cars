using Backend.DTOs;
using Backend.Models;
using Backend.Repository;

namespace Backend.Services
{
    public class CarService : ICommonService<CarDTOs, CarInsertDTOs, CarUpdateDTOs>
    {
        private readonly IRepository<Car> _repository;

        public CarService(IRepository<Car> repository)
        {
            _repository = repository;
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
                Year = car.Year
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
                Year = car.Year
            };
        }

        public async Task<CarDTOs> Add(CarInsertDTOs dto)
        {
            var car = new Car
            {
                BrandID = dto.BrandID,
                Milles = dto.Milles,
                Model = dto.Model,
                Year = dto.Year
            };

            await _repository.Add(car);
            await _repository.Save();

            return new CarDTOs
            {
                Id = car.CarID,
                BrandID = car.BrandID,
                Milles = car.Milles,
                Model = car.Model,
                Year = car.Year
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
                Year = car.Year
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
                Year = car.Year
            };
        }
    }
}
