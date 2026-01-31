using BankingSolution.Application.Features.Accounts.Queries.GetBalanceByAccount;
using BankingSolution.Application.UnitTest.Mocks;
using BankingSolution.Domain.Entities;
using BankingSolution.Infrastructure.Repositories;
using Moq;
using Shouldly;
using Xunit;

namespace BankingSolution.Application.UnitTest.Features.Accounts.Queries
{
    public class GetBalanceByAccountQueryHandlerXUnitTests
    {
        private readonly Mock<UnitOfWork> _unitOfWork;

        public GetBalanceByAccountQueryHandlerXUnitTests()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
        }

        private GetBalanceByAccountQueryHandler CreateHandler()
        {
            return new GetBalanceByAccountQueryHandler(
                _unitOfWork.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Return_Balance_When_Account_Exists()
        {
            // Arrange
            var handler = CreateHandler();

            var account = new BankAccount
            {
                Id = Guid.NewGuid(),
                AccountNumber = "ACC-123456",
                Balance = 250.75m
            };

            _unitOfWork.Object.BankingSolutionDbContext.BankAccounts!.Add(account);
            await _unitOfWork.Object.BankingSolutionDbContext.SaveChangesAsync();

            var query = new GetBalanceByAccountQuery("ACC-123456");

            // Act
            var balance = await handler.Handle(query, CancellationToken.None);

            // Assert
            balance.ShouldBe(250.75m);
        }
    }
}
