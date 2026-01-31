using AutoMapper;
using BankingSolution.Application.Features.Accounts.Commands.CreateAccount;
using BankingSolution.Application.Mappings;
using BankingSolution.Application.UnitTest.Mocks;
using BankingSolution.Domain.Entities;
using BankingSolution.Infrastructure.Repositories;
using Moq;
using Shouldly;
using Xunit;

namespace BankingSolution.Application.UnitTest.Features.Accounts.Commands
{
   
    public class CreateAccountCommandHandlerXUnitTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;

        public CreateAccountCommandHandlerXUnitTests()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        private CreateAccountCommandHandler CreateHandler()
        {
            return new CreateAccountCommandHandler(
                _unitOfWork.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Handle_Should_Create_Bank_Account()
        {
            // Arrange
            var handler = CreateHandler();

            var client = new Client
            {
                Id = Guid.NewGuid(),
                Name = "Test Client",
                Sex = "M"
            };

            // Insertamos solo el cliente necesario para la prueba
            _unitOfWork.Object.BankingSolutionDbContext.Clients!.Add(client);
            await _unitOfWork.Object.BankingSolutionDbContext.SaveChangesAsync();

            var command = new CreateAccountCommand
            {
                ClientId = client.Id,
                InitialBalance = 100m
            };

            // Act
            var accountId = await handler.Handle(command, CancellationToken.None);

            // Assert
            accountId.ShouldNotBe(Guid.Empty);

            var account = _unitOfWork.Object.BankingSolutionDbContext.BankAccounts!
                .FirstOrDefault(a => a.Id == accountId);

            account.ShouldNotBeNull();
            account!.ClientId.ShouldBe(client.Id);
            account.Balance.ShouldBe(100m);
            account.AccountNumber.ShouldStartWith("ACC-");
        }
    }
}
