using BankingSolution.Application.Features.Accounts.Querys.Vms;
using MediatR;

namespace BankingSolution.Application.Features.Accounts.Commands.CreateDeposit
{
    public class DepositCommand : IRequest<TransactionVm>
    {
        public string AccountNumber { get; set; } = null!;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
