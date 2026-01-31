using AutoMapper;
using BankingSolution.Application.Features.Transactions.Commands.CreateWithdraw;
using BankingSolution.Application.Mappings;
using BankingSolution.Application.UnitTest.Mocks;
using BankingSolution.Domain.Entities;
using BankingSolution.Domain.Enum;
using BankingSolution.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using Xunit;

namespace BankingSolution.Application.UnitTest.Features.Transactions.Commands.CreateWithdraw
{
    public class WithdrawCommandHandlerXUnitTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;

        public WithdrawCommandHandlerXUnitTests()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        private WithdrawCommandHandler CreateHandler()
        {
            return new WithdrawCommandHandler(
                _unitOfWork.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Handle_Should_Withdraw_Amount_And_Return_TransactionVm()
        {
            // Arrange
            var handler = CreateHandler();

            var account = new BankAccount
            {
                Id = Guid.NewGuid(),
                AccountNumber = "ACC-WITH-001",
                Balance = 200m
            };

            _unitOfWork.Object.BankingSolutionDbContext.BankAccounts!.Add(account);
            await _unitOfWork.Object.BankingSolutionDbContext.SaveChangesAsync();

            _unitOfWork.Object.BankingSolutionDbContext.Entry(account).State = EntityState.Detached;

            var command = new WithdrawCommand
            {
                AccountNumber = "ACC-WITH-001",
                Amount = 50m,
                Description = "Retiro de prueba"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Amount.ShouldBe(50m);
            result.Type.ShouldBe(TransactionType.Withdrawal.ToString());
            result.Description.ShouldBe("Retiro de prueba");

            var updatedAccount = _unitOfWork.Object.BankingSolutionDbContext.BankAccounts!
                .First(a => a.AccountNumber == "ACC-WITH-001");

            updatedAccount.Balance.ShouldBe(150m);

            var transaction = _unitOfWork.Object.BankingSolutionDbContext.Transactions!
                .FirstOrDefault(t => t.BankAccountId == account.Id);

            transaction.ShouldNotBeNull();
            transaction!.BalanceAfter.ShouldBe(150m);
        }
    }
}
