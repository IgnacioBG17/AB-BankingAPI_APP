namespace BankingSolution.Application.Features.Accounts.Querys.Vms
{
    public class TransactionVm
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
    }
}
