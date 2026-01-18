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
        private readonly CarService _carService;

        public CarController(
            IValidator<CarInsertDTOs> carInsertValidator,
            IValidator<CarUpdateDTOs> carUpdateValidator,
            CarService carService
        )
        {
            _carInsertValidator = carInsertValidator;
            _carUpdateValidator = carUpdateValidator;
            _carService = carService;
        }

        /* =======================
           GET ALL
        ======================= */
        [HttpGet]
        public async Task<IEnumerable<CarDTOs>> Get()
        {
            return await _carService.Get();
        }

        /* =======================
           GET BY ID
        ======================= */
        [HttpGet("{id}")]
        public async Task<ActionResult<CarDTOs>> GetById(int id)
        {
            var car = await _carService.GetById(id);
            return car == null ? NotFound() : Ok(car);
        }

        /* =======================
           CREATE
        ======================= */
        [HttpPost]
        public async Task<ActionResult<CarDTOs>> Add([FromForm] CarInsertDTOs dto)
        {
            var validation = await _carInsertValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var car = await _carService.Add(dto);
            return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
        }

        /* =======================
           UPDATE
        ======================= */
        [HttpPut("{id}")]
        public async Task<ActionResult<CarDTOs>> Update(int id, [FromForm] CarUpdateDTOs dto)
        {
            var validation = await _carUpdateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var car = await _carService.Update(id, dto);
            return car == null ? NotFound() : Ok(car);
        }

        /* =======================
           DELETE ONE
        ======================= */
        [HttpDelete("{id}")]
        public async Task<ActionResult<CarDTOs>> Delete(int id)
        {
            var car = await _carService.Delete(id);
            return car == null ? NotFound() : Ok(car);
        }

        /* =======================
           DELETE ALL
        ======================= */
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAll()
        {
            var deletedCount = await _carService.DeleteAll();

            return Ok(new
            {
                message = "Todos los coches han sido eliminados",
                deleted = deletedCount
            });
        }
    }
}
