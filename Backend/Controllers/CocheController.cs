using System.Reflection.Metadata.Ecma335;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CocheController : ControllerBase
    {

        private ICocheService _cocheService;

        public CocheController( ICocheService cocheService )
        {
            _cocheService = cocheService;
        }

        [HttpGet("all")]
        public List<Coche> GetCoches()
        {
            return Repository.Coche;
        }

        [HttpGet("{id}")]
        public ActionResult<Coche> Get( int Id )
        {
            var coche = Repository.Coche.FirstOrDefault( coche => coche.Id == Id );
            if (coche == null)
            {
                return NotFound();
            }
            return Ok(coche);
        }

        [HttpGet("search/{search}")]
        public List<Coche> GetMarca(string search)
        { 
            return Repository.Coche.Where( coche => coche.Marca.ToUpper().Contains(search.ToUpper())).ToList();
        }

        [HttpPost("add")]
        public IActionResult Add(Coche coche)
        {
            if (!_cocheService.Validate(coche))
            {
                return BadRequest();
            }
            Repository.Coche.Add(coche);
            return NoContent();
        }
    }
        public class Repository
        {
            public static List<Coche> Coche = new List<Coche>
            {
                new Coche { Id = 1, Modelo = "Civic", Marca = "Honda", Año = 2018 },
                new Coche { Id = 2, Modelo = "Corolla", Marca = "Toyota", Año = 2020 },
                new Coche { Id = 3, Modelo = "Model S", Marca = "Tesla", Año = 2022 },
                new Coche { Id = 4, Modelo = "Mustang", Marca = "Ford", Año = 2017 },
                new Coche { Id = 5, Modelo = "3 Series", Marca = "BMW", Año = 2019 },
                new Coche { Id = 6, Modelo = "A4", Marca = "Audi", Año = 2021 },
                new Coche { Id = 7, Modelo = "Golf", Marca = "Volkswagen", Año = 2016 },
                new Coche { Id = 8, Modelo = "Altima", Marca = "Nissan", Año = 2015 },
                new Coche { Id = 9, Modelo = "Accord", Marca = "Honda", Año = 2023 },
                new Coche { Id = 10, Modelo = "Camry", Marca = "Toyota", Año = 2018 },
                new Coche { Id = 11, Modelo = "Rav4", Marca = "Toyota", Año = 2022 },
                new Coche { Id = 12, Modelo = "CX-5", Marca = "Mazda", Año = 2020 },
                new Coche { Id = 13, Modelo = "Cherokee", Marca = "Jeep", Año = 2019 },
                new Coche { Id = 14, Modelo = "F-150", Marca = "Ford", Año = 2021 },
                new Coche { Id = 15, Modelo = "Silverado", Marca = "Chevrolet", Año = 2017 },
                new Coche { Id = 16, Modelo = "Wrangler", Marca = "Jeep", Año = 2023 },
                new Coche { Id = 17, Modelo = "CX-9", Marca = "Mazda", Año = 2018 },
                new Coche { Id = 18, Modelo = "S60", Marca = "Volvo", Año = 2015 },
                new Coche { Id = 19, Modelo = "Kona", Marca = "Hyundai", Año = 2021 },
                new Coche { Id = 20, Modelo = "Tucson", Marca = "Hyundai", Año = 2019 },
                new Coche { Id = 21, Modelo = "Impala", Marca = "Chevrolet", Año = 2016 },
                new Coche { Id = 22, Modelo = "Optima", Marca = "Kia", Año = 2018 },
                new Coche { Id = 23, Modelo = "Sportage", Marca = "Kia", Año = 2020 },
                new Coche { Id = 24, Modelo = "Rogue", Marca = "Nissan", Año = 2022 },
                new Coche { Id = 25, Modelo = "3", Marca = "Mazda", Año = 2017 },
                new Coche { Id = 26, Modelo = "Explorer", Marca = "Ford", Año = 2019 },
                new Coche { Id = 27, Modelo = "Range Rover Evoque", Marca = "Land Rover", Año = 2021 },
                new Coche { Id = 28, Modelo = "Jetta", Marca = "Volkswagen", Año = 2018 },
                new Coche { Id = 29, Modelo = "Supra", Marca = "Toyota", Año = 2020 },
                new Coche { Id = 30, Modelo = "Polo", Marca = "Volkswagen", Año = 2016 }
            };
        }

    public class Coche
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public int Año { get; set; }
    }
    }