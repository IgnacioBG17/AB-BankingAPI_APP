using AutoMapper;
using BankingSolution.Application.Features.Transactions.Queries.GetTransactionsByAccount;
using BankingSolution.Application.Mappings;
using BankingSolution.Application.UnitTest.Mocks;
using BankingSolution.Domain.Entities;
using BankingSolution.Domain.Enum;
using BankingSolution.Infrastructure.Repositories;
using Moq;
using Shouldly;
using Xunit;

namespace BankingSolution.Application.UnitTest.Features.Transactions.Queries
{
    public class GetTransactionsByAccountQueryHandlerXUnitTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;

        public GetTransactionsByAccountQueryHandlerXUnitTests()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        private GetTransactionsByAccountQueryHandler CreateHandler()
        {
            return new GetTransactionsByAccountQueryHandler(
                _unitOfWork.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Handle_Should_Return_AccountStatement_With_Transactions()
        {
            // Arrange
            var handler = CreateHandler();

            var account = new BankAccount
            {
                Id = Guid.NewGuid(),
                AccountNumber = "ACC-STAT-001",
                Balance = 300m
            };

            _unitOfWork.Object.BankingSolutionDbContext.BankAccounts!.Add(account);
            await _unitOfWork.Object.BankingSolutionDbContext.SaveChangesAsync();

            var transaction1 = new Transaction
            {
                Id = Guid.NewGuid(),
                BankAccountId = account.Id,
                Type = TransactionType.Deposit,
                Amount = 200m,
                BalanceAfter = 200m,
                CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                Description = "Depósito"
            };

            var transaction2 = new Transaction
            {
                Id = Guid.NewGuid(),
                BankAccountId = account.Id,
                Type = TransactionType.Withdrawal,
                Amount = 100m,
                BalanceAfter = 100m,
                CreatedAt = DateTime.UtcNow.AddMinutes(-5),
                Description = "Retiro"
            };

            _unitOfWork.Object.BankingSolutionDbContext.Transactions!.AddRange(transaction1, transaction2);
            await _unitOfWork.Object.BankingSolutionDbContext.SaveChangesAsync();

            var query = new GetTransactionsByAccountQuery("ACC-STAT-001");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.AccountNumber.ShouldBe("ACC-STAT-001");
            result.FinalBalance.ShouldBe(300m);

            result.Transactions.ShouldNotBeNull();
            result.Transactions.Count.ShouldBe(2);

            result.Transactions.First().Type.ShouldBe(TransactionType.Deposit.ToString());
            result.Transactions.Last().Type.ShouldBe(TransactionType.Withdrawal.ToString());
        }
    }
}
