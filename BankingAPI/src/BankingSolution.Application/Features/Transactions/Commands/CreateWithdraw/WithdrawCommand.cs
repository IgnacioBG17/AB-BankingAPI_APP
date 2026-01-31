using BankingSolution.Application.Features.Transactions.Queries.Vms;
using MediatR;

namespace BankingSolution.Application.Features.Transactions.Commands.CreateWithdraw
{
    public class WithdrawCommand : IRequest<TransactionVm>
    {
        public string AccountNumber { get; set; } = null!;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
