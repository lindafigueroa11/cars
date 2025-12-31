using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class CarService : ICommonService<CarDTOs, CarInsertDTOs, CarUpdateDTOs>
    {
        private readonly IRepository<Car> _carRepository;

        public CarService(IRepository<Car> carRepository)
        {
            _carRepository = carRepository; // ✅
        }

        public async Task<IEnumerable<CarDTOs>> Get()
        {
            var cars = await _carRepository.Get();

            return cars.Select(car => new CarDTOs() 
            {
                 Id = car.CarID,
                 BrandID = car.BrandID,
                 Milles = car.Milles,
                 Model = car.Model
            });
        }

        public async Task<CarDTOs> Add(CarInsertDTOs carInsertDTOs)
        {
            var car = new Car
            {
                BrandID = carInsertDTOs.BrandID,
                Milles = carInsertDTOs.Milles,
                Model = carInsertDTOs.Model,
                Year = carInsertDTOs.Year,

            };

            var carDTO = new CarDTOs
            {
                Id = car.CarID,
                BrandID = car.BrandID,
                Milles = car.Milles,
                Model = car.Model,
                Year = car.Year
            };

            await _carRepository.Add(car);
            await _carRepository.Save();
            return carDTO;
        }

        public async Task<CarDTOs> Update(int Id, CarUpdateDTOs carUpdateDTOs)
        {

            var car = await _carRepository.GetById(Id);
            
            if (car != null)
            {
                car.BrandID = carUpdateDTOs.BrandID;
                car.Milles = carUpdateDTOs.Milles;
                car.Model = carUpdateDTOs.Model;
                car.Year = carUpdateDTOs.Year;

                _carRepository.Update(car);
                await _carRepository.Save();

                var carDTO = new CarDTOs
                {
                    Id = car.CarID,
                    Milles = car.Milles,
                    Model = car.Model,
                    Year = car.Year,
                };

                return carDTO;
               
            }
            return null;

        }
        public async Task<CarDTOs> Delete(int Id)
        {
            var car = await _carRepository.GetById(Id);

            if (car == null) return null;
                
            var carDTO = new CarDTOs
                {
                    Id = car.CarID,
                    Milles = car.Milles,
                    Model = car.Model,
                    Year = car.Year,
                };

            _carRepository.Delete(car);
            await _carRepository.Save();

            return carDTO;
        }

        public async Task<CarDTOs> GetById(int Id)
        {
            var car = await _carRepository.GetById(Id);
            if (car != null)
            {
                var carDTO = new CarDTOs
                {
                    Id = car.CarID,
                    Milles = car.Milles,
                    Model = car.Model,
                    Year = car.Year,
                };
                return carDTO;
            }

            return null;
        }

    }
}
