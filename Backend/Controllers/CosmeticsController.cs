using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CosmeticsController : ControllerBase
    {
        [HttpGet("all")]
        public List<Cosmetic> GetCosmetics()
        {
            return RepositoryCosm.Cosmetic;
        }

        [HttpGet("{id}")]
        public ActionResult<Cosmetic> Get(int Id)
        {
            var Cosmetic = RepositoryCosm.Cosmetic.FirstOrDefault(Cosmetic => Cosmetic.Id == Id);
            if (Cosmetic == null)
            {
                return NotFound();
            }
            return Ok(Cosmetic);
        }

        [HttpGet("search/{search}")]
        public List<Cosmetic> GetName(string search)
        {
            return RepositoryCosm.Cosmetic.Where(coche => coche.Name.ToUpper().Contains(search.ToUpper())).ToList();
        }

        [HttpGet("search/marca/{search}")]
        public List<Cosmetic> GetMarca(string search)
        {
            return RepositoryCosm.Cosmetic.Where(coche => coche.Marca.ToUpper().Contains(search.ToUpper())).ToList();
        }

        [HttpGet("search/modelo/{search}")]
        public List<Cosmetic> GetModelo(string search)
        {
            return RepositoryCosm.Cosmetic.Where(coche => coche.Modelo.ToUpper().Contains(search.ToUpper())).ToList();
        }

        [HttpPost("add")]
        public IActionResult Add(Cosmetic cosmetic)
        {
            if (string.IsNullOrEmpty(cosmetic.Marca))
            {
                return BadRequest();
            }
            RepositoryCosm.Cosmetic.Add(cosmetic);
            return NoContent();
        }
    }
    public class RepositoryCosm
    {
        public static List<Cosmetic> Cosmetic = new List<Cosmetic>
            {
                new Cosmetic { Id = 1, Name = "Base líquida", Modelo = "Fit Me", Marca = "Maybelline", Categoria = "Maquillaje Rostro", Precio = 180 },
                new Cosmetic { Id = 2, Name = "Corrector", Modelo = "Instant Age Rewind", Marca = "Maybelline", Categoria = "Maquillaje Rostro", Precio = 150 },
                new Cosmetic { Id = 3, Name = "Polvo compacto", Modelo = "True Match", Marca = "L'Oréal", Categoria = "Maquillaje Rostro", Precio = 220 },
                new Cosmetic { Id = 4, Name = "Rubor", Modelo = "Orgasm", Marca = "NARS", Categoria = "Maquillaje Rostro", Precio = 450 },
                new Cosmetic { Id = 5, Name = "Iluminador", Modelo = "Glow Kit", Marca = "Anastasia Beverly Hills", Categoria = "Maquillaje Rostro", Precio = 520 },
                new Cosmetic { Id = 6, Name = "Paleta de sombras", Modelo = "Nude Heat", Marca = "Urban Decay", Categoria = "Maquillaje Ojos", Precio = 650 },
                new Cosmetic { Id = 7, Name = "Delineador líquido", Modelo = "Tattoo Liner", Marca = "Kat Von D", Categoria = "Maquillaje Ojos", Precio = 380 },
                new Cosmetic { Id = 8, Name = "Máscara de pestañas", Modelo = "Lash Sensational", Marca = "Maybelline", Categoria = "Maquillaje Ojos", Precio = 170 },
                new Cosmetic { Id = 9, Name = "Lápiz de cejas", Modelo = "Precisely My Brow", Marca = "Benefit", Categoria = "Maquillaje Ojos", Precio = 410 },
                new Cosmetic { Id = 10, Name = "Gel para cejas", Modelo = "Brow Gel", Marca = "Essence", Categoria = "Maquillaje Ojos", Precio = 95 },
                new Cosmetic { Id = 11, Name = "Labial mate", Modelo = "Velvet Teddy", Marca = "MAC", Categoria = "Maquillaje Labios", Precio = 420 },
                new Cosmetic { Id = 12, Name = "Gloss labial", Modelo = "Lifter Gloss", Marca = "Maybelline", Categoria = "Maquillaje Labios", Precio = 160 },
                new Cosmetic { Id = 13, Name = "Bálsamo labial", Modelo = "Original Care", Marca = "Nivea", Categoria = "Maquillaje Labios", Precio = 60 },
                new Cosmetic { Id = 14, Name = "Tinta labial", Modelo = "Lip Tint", Marca = "Peripera", Categoria = "Maquillaje Labios", Precio = 190 },
                new Cosmetic { Id = 15, Name = "Delineador de labios", Modelo = "Lip Pencil", Marca = "NYX", Categoria = "Maquillaje Labios", Precio = 140 },
                new Cosmetic { Id = 16, Name = "Primer facial", Modelo = "Pore Filler", Marca = "NYX", Categoria = "Skincare", Precio = 260 },
                new Cosmetic { Id = 17, Name = "Spray fijador", Modelo = "All Nighter", Marca = "Urban Decay", Categoria = "Skincare", Precio = 480 },
                new Cosmetic { Id = 18, Name = "Crema hidratante", Modelo = "Hydro Boost", Marca = "Neutrogena", Categoria = "Skincare", Precio = 230 },
                new Cosmetic { Id = 19, Name = "Sérum facial", Modelo = "Hyaluronic Acid", Marca = "The Ordinary", Categoria = "Skincare", Precio = 190 },
                new Cosmetic { Id = 20, Name = "Protector solar", Modelo = "Anthelios", Marca = "La Roche-Posay", Categoria = "Skincare", Precio = 380 },
                new Cosmetic { Id = 21, Name = "Mascarilla facial", Modelo = "Charcoal Mask", Marca = "Garnier", Categoria = "Skincare", Precio = 120 },
                new Cosmetic { Id = 22, Name = "Agua micelar", Modelo = "Micellar Water", Marca = "Garnier", Categoria = "Skincare", Precio = 120 },
                new Cosmetic { Id = 23, Name = "Exfoliante facial", Modelo = "Fresh Scrub", Marca = "L'Oréal", Categoria = "Skincare", Precio = 210 },
                new Cosmetic { Id = 24, Name = "Tónico facial", Modelo = "Rose Water", Marca = "Pixi", Categoria = "Skincare", Precio = 280 },
                new Cosmetic { Id = 25, Name = "Crema antiarrugas", Modelo = "Revitalift", Marca = "L'Oréal", Categoria = "Skincare", Precio = 390 },
                new Cosmetic { Id = 26, Name = "Shampoo", Modelo = "Hidra Rizos", Marca = "Garnier", Categoria = "Cabello", Precio = 135 },
                new Cosmetic { Id = 27, Name = "Acondicionador", Modelo = "Pro-V", Marca = "Pantene", Categoria = "Cabello", Precio = 140 },
                new Cosmetic { Id = 28, Name = "Mascarilla capilar", Modelo = "Elvive Repair", Marca = "L'Oréal", Categoria = "Cabello", Precio = 190 },
                new Cosmetic { Id = 29, Name = "Aceite capilar", Modelo = "Argan Oil", Marca = "OGX", Categoria = "Cabello", Precio = 220 },
                new Cosmetic { Id = 30, Name = "Crema para peinar", Modelo = "Rizos Definidos", Marca = "Sedal", Categoria = "Cabello", Precio = 110 },
                new Cosmetic { Id = 31, Name = "Gel fijador", Modelo = "Extreme Hold", Marca = "Got2b", Categoria = "Cabello", Precio = 150 },
                new Cosmetic { Id = 32, Name = "Spray capilar", Modelo = "Elnett", Marca = "L'Oréal", Categoria = "Cabello", Precio = 260 },
                new Cosmetic { Id = 33, Name = "Espuma para rizos", Modelo = "Curl Defining", Marca = "TRESemmé", Categoria = "Cabello", Precio = 170 },
                new Cosmetic { Id = 34, Name = "Tinte para cabello", Modelo = "Casting Crème Gloss", Marca = "L'Oréal", Categoria = "Cabello", Precio = 210 },
                new Cosmetic { Id = 35, Name = "Tratamiento capilar", Modelo = "Olaplex No.3", Marca = "Olaplex", Categoria = "Cabello", Precio = 650 },
                new Cosmetic { Id = 36, Name = "Crema para ojos", Modelo = "Eye Repair", Marca = "Olay", Categoria = "Skincare", Precio = 320 },
                new Cosmetic { Id = 37, Name = "Esencia facial", Modelo = "Advanced Snail", Marca = "COSRX", Categoria = "Skincare", Precio = 350 },
                new Cosmetic { Id = 38, Name = "Jabón facial", Modelo = "Foaming Cleanser", Marca = "CeraVe", Categoria = "Skincare", Precio = 190 },
                new Cosmetic { Id = 39, Name = "Bruma facial", Modelo = "Fix+ Rose", Marca = "MAC", Categoria = "Skincare", Precio = 420 },
                new Cosmetic { Id = 40, Name = "Parche para acné", Modelo = "Acne Patch", Marca = "COSRX", Categoria = "Skincare", Precio = 140 },
                new Cosmetic { Id = 41, Name = "Pestañas postizas", Modelo = "Demi Wispies", Marca = "Ardell", Categoria = "Maquillaje Ojos", Precio = 160 },
                new Cosmetic { Id = 42, Name = "Sombras individuales", Modelo = "Single Shadow", Marca = "ColourPop", Categoria = "Maquillaje Ojos", Precio = 120 },
                new Cosmetic { Id = 43, Name = "Bronzer", Modelo = "Hoola", Marca = "Benefit", Categoria = "Maquillaje Rostro", Precio = 520 },
                new Cosmetic { Id = 44, Name = "Contorno facial", Modelo = "Match Stix", Marca = "Fenty Beauty", Categoria = "Maquillaje Rostro", Precio = 490 },
                new Cosmetic { Id = 45, Name = "BB Cream", Modelo = "BB Cream", Marca = "Garnier", Categoria = "Maquillaje Rostro", Precio = 140 },
                new Cosmetic { Id = 46, Name = "Aceite facial", Modelo = "Rosehip Oil", Marca = "The Ordinary", Categoria = "Skincare", Precio = 210 },
                new Cosmetic { Id = 47, Name = "Crema matificante", Modelo = "Oil Control", Marca = "La Roche-Posay", Categoria = "Skincare", Precio = 360 },
                new Cosmetic { Id = 48, Name = "Bálsamo limpiador", Modelo = "Clean It Zero", Marca = "Banila Co", Categoria = "Skincare", Precio = 390 },
                new Cosmetic { Id = 49, Name = "Tratamiento de keratina", Modelo = "Keratina Líquida", Marca = "Novex", Categoria = "Cabello", Precio = 180 },
                new Cosmetic { Id = 50, Name = "Crema iluminadora", Modelo = "Glow Recipe", Marca = "Glow Recipe", Categoria = "Skincare", Precio = 480 }
            };
        }

    public class Cosmetic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public int Precio { get; set; }
    }
}