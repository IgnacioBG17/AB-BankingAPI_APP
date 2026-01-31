namespace BankingSolution.Domain.Entities
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; } = null!;
        public decimal Balance { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
