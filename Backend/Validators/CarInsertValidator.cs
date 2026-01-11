using Backend.DTOs;
using FluentValidation;

namespace Backend.Validators
{
    public class CarInsertValidator : AbstractValidator<CarInsertDTOs>
    { 
        public CarInsertValidator(){
            RuleFor(x => x.Year).GreaterThan(1950).NotEmpty().NotNull().WithMessage("El año es obligatorio");
            RuleFor(x => x.BrandID).NotEmpty().NotNull().WithMessage("El Identificador de la marca es obligatorio");
            RuleFor(x => x.Milles).GreaterThan(0).NotEmpty().NotNull().WithMessage("El kilometraje es obligatorio");
            RuleFor(x => x.Model).Length(1, 20).NotEmpty().NotNull().WithMessage("El Modelo es obligatorio");
            RuleFor(x => x.Image)
            .Must(file => file == null || file.ContentType.StartsWith("image/"))
            .WithMessage("El archivo debe ser una imagen")
            .Must(file => file == null || file.Length <= 5_000_000)
            .WithMessage("La imagen no debe superar 5MB");
        }
    }
}
