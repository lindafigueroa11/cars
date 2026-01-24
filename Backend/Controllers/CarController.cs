using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/cars")]
    public class CarController : ControllerBase
    {
        private readonly CarService _service;

        public CarController(CarService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _service.Get());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var car = await _service.GetById(id);
            return car == null ? NotFound() : Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CarInsertDTOs dto)
            => Ok(await _service.Add(dto));

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAll()
            => Ok(await _service.DeleteAll());
    }
}
