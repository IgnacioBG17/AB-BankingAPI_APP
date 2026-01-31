using BankingSolution.Application.Features.Transactions.Queries.Vms;
using MediatR;

namespace BankingSolution.Application.Features.Transactions.Commands.CreateDeposit
{
    public class DepositCommand : IRequest<TransactionVm>
    {
        public string AccountNumber { get; set; } = null!;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
