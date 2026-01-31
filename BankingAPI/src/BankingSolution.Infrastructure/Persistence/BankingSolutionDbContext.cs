using BankingSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSolution.Infrastructure.Persistence
{
    public class BankingSolutionDbContext : DbContext
    {
        public BankingSolutionDbContext(DbContextOptions<BankingSolutionDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<BankAccount> BankAccounts { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Client>(eb =>
            {
                eb.ToTable("Clients");

                eb.HasKey(c => c.Id);

                eb.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(200);

                eb.Property(c => c.Sex)
                  .IsRequired()
                  .HasMaxLength(10);

                eb.Property(c => c.Income)
                  .HasColumnType("decimal(18,2)");

                eb.HasMany(c => c.Accounts)
                  .WithOne(a => a.Client)
                  .HasForeignKey(a => a.ClientId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<BankAccount>(eb =>
            {
                eb.ToTable("BankAccounts");

                eb.HasKey(a => a.Id);

                eb.Property(a => a.AccountNumber)
                  .IsRequired()
                  .HasMaxLength(50);

                eb.HasIndex(a => a.AccountNumber)
                  .IsUnique();

                eb.Property(a => a.Balance)
                  .HasColumnType("decimal(18,2)")
                  .HasDefaultValue(0m);

                eb.HasMany(a => a.Transactions)
                  .WithOne(t => t.BankAccount)
                  .HasForeignKey(t => t.BankAccountId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Transaction>(eb =>
            {
                eb.ToTable("Transactions");

                eb.HasKey(t => t.Id);

                eb.Property(t => t.Type)
                  .IsRequired();

                eb.Property(t => t.Amount)
                  .HasColumnType("decimal(18,2)");

                eb.Property(t => t.BalanceAfter)
                  .HasColumnType("decimal(18,2)");

                eb.Property(t => t.CreatedAt)
                  .IsRequired();

                eb.Property(t => t.Description)
                  .HasMaxLength(500);
            });
        }

    }
}
