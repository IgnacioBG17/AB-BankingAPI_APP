using MediatR;

namespace BankingSolution.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<Guid>
    {
        public Guid ClientId { get; set; }
        public decimal InitialBalance { get; set; }
    }
}
