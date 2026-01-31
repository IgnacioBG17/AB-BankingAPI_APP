namespace BankingSolution.Application.Features.Accounts.Querys.Vms
{
    public class AccountStatementVm
    {
        public string AccountNumber { get; set; } = null!;
        public decimal FinalBalance { get; set; } 
        public List<TransactionVm> Transactions { get; set; } = new();
    }
}
