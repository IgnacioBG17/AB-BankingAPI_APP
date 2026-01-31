using FluentValidation;

namespace BankingSolution.Application.Features.Accounts.Commands.CreateClient
{
    public class CreateClientCommandValidator
        : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("El nombre no puede estar vacío")
                .MaximumLength(200).WithMessage("El nombre no puede exceder los 200 caracteres");

            RuleFor(c => c.Sex)
                .NotEmpty().WithMessage("El sexo es requerido")
                .Must(s => s == "M" || s == "F")
                .WithMessage("El sexo debe ser 'M' o 'F'");

            RuleFor(c => c.DateOfBirth)
                .LessThan(DateTime.Today)
                .WithMessage("La fecha de nacimiento debe ser válida");

            RuleFor(c => c.Income)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El ingreso no puede ser negativo");
        }
    }
}
