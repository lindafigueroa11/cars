using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Backend.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private IValidator<CarInsertDTOs> _carInsertValidator;
        private IValidator<CarUpdateDTOs> _carUpdateValidator;
        private ICommonService<CarDTOs, CarInsertDTOs, CarUpdateDTOs> _carService;

        public CarController(StoreContext context,
            IValidator<CarInsertDTOs> carInsertValidator,
            IValidator<CarUpdateDTOs> carUpdateValidator,
            [FromKeyedServices("carService")] ICommonService<CarDTOs, CarInsertDTOs, CarUpdateDTOs> carService)
        {
            _carInsertValidator = carInsertValidator;
            _carUpdateValidator = carUpdateValidator;
            _carService = carService;
        }

        [HttpGet]
        public async Task<IEnumerable<CarDTOs>> Get()
        {
            return await _carService.Get(); 
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<CarDTOs>> GetById(int Id)
        {
            var carDTO = await _carService.GetById(Id);

            return carDTO == null ? NotFound() : Ok(carDTO);

        }

        [HttpPost]
        public async Task<ActionResult<CarDTOs>> Add(CarInsertDTOs carInsertDTOs)
        {
            var validationResult = await _carInsertValidator.ValidateAsync(carInsertDTOs);

            if (!validationResult.IsValid) 
            {
                return BadRequest(validationResult.Errors);
            }



            var carDTO = await _carService.Add(carInsertDTOs);

            return CreatedAtAction(
                nameof(GetById),
                new { Id = carDTO.Id},
                carDTO
            );
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<CarDTOs>> Update(int Id, CarUpdateDTOs carUpdateDTOs)
        {
            var validationResult = await _carUpdateValidator.ValidateAsync(carUpdateDTOs);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var carDTO = await _carService.Update(Id, carUpdateDTOs);

            return carDTO == null ? NotFound() : Ok(carDTO);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CarDTOs>> Delete(int Id)
        {
            var carDTO = await _carService.Delete(Id);

            if (carDTO == null) NotFound();

            return carDTO;
        }

    }
}