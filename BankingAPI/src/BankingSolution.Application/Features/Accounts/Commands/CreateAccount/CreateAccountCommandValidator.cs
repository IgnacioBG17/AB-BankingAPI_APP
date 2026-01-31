using FluentValidation;

namespace BankingSolution.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator
        : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(a => a.ClientId)
                .NotEmpty().WithMessage("El ClientId es requerido");

            RuleFor(a => a.InitialBalance)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El saldo inicial no puede ser negativo");
        }
    }
}
