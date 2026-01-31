using BankingSolution.Application.Features.Transactions.Queries.Vms;
using MediatR;

namespace BankingSolution.Application.Features.Transactions.Queries.GetTransactionsByAccount
{
    public class GetTransactionsByAccountQuery : IRequest<AccountStatementVm>
    {
        public string AccountNumber { get; set; }

        public GetTransactionsByAccountQuery(string accountNumber)
        {
            AccountNumber = string.IsNullOrWhiteSpace(accountNumber)
                ? throw new ArgumentNullException(nameof(accountNumber))
                : accountNumber;
        }
    }
}
