using BankingSolution.Domain.Entities;
using BankingSolution.Domain.Enum;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BankingSolution.Infrastructure.Persistence
{
    public class BankingSolutionDbContextData
    {
        public static async Task LoadDataAsync(
          BankingSolutionDbContext context,
          ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Clients.Any())
                {
                    var clientData = File.ReadAllText(
                        "../BankingSolution.Infrastructure/Data/clients.json");

                    var clients = JsonConvert.DeserializeObject<List<Client>>(clientData);

                    await context.Clients.AddRangeAsync(clients!);
                    await context.SaveChangesAsync();
                }

                if (!context.BankAccounts.Any())
                {
                    var accountData = File.ReadAllText(
                        "../BankingSolution.Infrastructure/Data/bankaccounts.json");

                    var accounts = JsonConvert.DeserializeObject<List<BankAccount>>(accountData);

                    await context.BankAccounts.AddRangeAsync(accounts!);
                    await context.SaveChangesAsync();
                }

                if (!context.Transactions.Any())
                {
                    var transactionData = File.ReadAllText(
                        "../BankingSolution.Infrastructure/Data/transactions.json");

                    var transactionsSeed =
                        JsonConvert.DeserializeObject<List<TransactionSeedModel>>(transactionData);

                    var transactions = transactionsSeed!.Select(t => new Transaction
                    {
                        Id = t.Id,
                        BankAccountId = t.BankAccountId,
                        Type = Enum.Parse<TransactionType>(t.Type),
                        Amount = t.Amount,
                        BalanceAfter = t.BalanceAfter,
                        CreatedAt = t.CreatedAt,
                        Description = t.Description
                    }).ToList();

                    await context.Transactions.AddRangeAsync(transactions);
                    await context.SaveChangesAsync();

                    foreach (var account in context.BankAccounts)
                    {
                        var lastTx = transactions
                            .Where(t => t.BankAccountId == account.Id)
                            .OrderBy(t => t.CreatedAt)
                            .LastOrDefault();

                        if (lastTx != null)
                        {
                            account.Balance = lastTx.BalanceAfter;
                            context.BankAccounts.Update(account);
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<BankingSolutionDbContextData>();
                logger.LogError(e, "Error cargando datos iniciales");
            }
        }
        private class TransactionSeedModel
        {
            public Guid Id { get; set; }
            public Guid BankAccountId { get; set; }
            public string Type { get; set; } = null!;
            public decimal Amount { get; set; }
            public decimal BalanceAfter { get; set; }
            public DateTime CreatedAt { get; set; }
            public string? Description { get; set; }
        }
    }
}
