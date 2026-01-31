using MediatR;

namespace BankingSolution.Application.Features.Accounts.Querys.GetBalanceByAccount
{
    public class GetBalanceByAccountQuery : IRequest<decimal>
    {
        public string AccountNumber { get; set; }

        public GetBalanceByAccountQuery(string accountNumber)
        {
            AccountNumber = string.IsNullOrWhiteSpace(accountNumber)
                ? throw new ArgumentNullException(nameof(accountNumber))
                : accountNumber;
        }
    }
}
