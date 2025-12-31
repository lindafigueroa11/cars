using Backend.Controllers;

namespace Backend.Services
{
    public class CocheService : ICocheService
    {
        public bool Validate(Coche coche)
        {
            if (string.IsNullOrEmpty(coche.Marca)||
                coche.Marca.Length > 15)
            {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
