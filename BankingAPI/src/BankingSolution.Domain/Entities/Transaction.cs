using BankingSolution.Domain.Enum;

namespace BankingSolution.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; } = null!;
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
    }
}
