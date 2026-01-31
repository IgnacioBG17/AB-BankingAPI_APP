namespace BankingSolution.Domain.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; } = null!;
        public decimal Income { get; set; }
        public ICollection<BankAccount> Accounts { get; set; } = new List<BankAccount>();
    }
}
