using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/car-location")]
    public class CarLocationController : ControllerBase
    {
        private readonly CarLocationService _service;

        public CarLocationController(CarLocationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CarLocationInsertDTOs dto)
        {
            await _service.Add(dto);
            return Ok();
        }

        [HttpGet("map")]
        public async Task<IActionResult> GetForMap()
        {
            var result = await _service.GetForMap();
            return Ok(result);
        }
    }
}
