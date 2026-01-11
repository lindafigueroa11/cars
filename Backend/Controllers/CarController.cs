using Backend.DTOs;
using Backend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly IValidator<CarInsertDTOs> _carInsertValidator;
        private readonly IValidator<CarUpdateDTOs> _carUpdateValidator;
        private readonly ICommonService<CarDTOs, CarInsertDTOs, CarUpdateDTOs> _carService;

        public CarController(
            IValidator<CarInsertDTOs> carInsertValidator,
            IValidator<CarUpdateDTOs> carUpdateValidator,
            [FromKeyedServices("carService")]
            ICommonService<CarDTOs, CarInsertDTOs, CarUpdateDTOs> carService)
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

        [HttpGet("{id}")]
        public async Task<ActionResult<CarDTOs>> GetById(int id)
        {
            var car = await _carService.GetById(id);
            return car == null ? NotFound() : Ok(car);
        }

        [HttpPost]
        public async Task<ActionResult<CarDTOs>> Add(CarInsertDTOs dto)
        {
            var validation = await _carInsertValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var car = await _carService.Add(dto);
            return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CarDTOs>> Update(int id, CarUpdateDTOs dto)
        {
            var validation = await _carUpdateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var car = await _carService.Update(id, dto);
            return car == null ? NotFound() : Ok(car);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CarDTOs>> Delete(int id)
        {
            var car = await _carService.Delete(id);
            return car == null ? NotFound() : Ok(car);
        }
    }
}