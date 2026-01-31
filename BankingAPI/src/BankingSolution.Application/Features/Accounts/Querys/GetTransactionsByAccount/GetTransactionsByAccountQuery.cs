using BankingSolution.Application.Features.Accounts.Querys.Vms;
using MediatR;

namespace BankingSolution.Application.Features.Accounts.Querys.GetTransactionsByAccount
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
