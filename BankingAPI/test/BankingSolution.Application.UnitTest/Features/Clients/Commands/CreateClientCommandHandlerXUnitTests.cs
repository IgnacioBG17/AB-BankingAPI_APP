using AutoMapper;
using BankingSolution.Application.Features.Clients.Commands.CreateClient;
using BankingSolution.Application.Mappings;
using BankingSolution.Application.UnitTest.Mocks;
using BankingSolution.Infrastructure.Repositories;
using Moq;
using Shouldly;
using Xunit;

namespace BankingSolution.Application.UnitTest.Features.Clients.Commands
{
    public class CreateClientCommandHandlerXUnitTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;

        public CreateClientCommandHandlerXUnitTests()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        private CreateClientCommandHandler CreateHandler()
        {
            return new CreateClientCommandHandler(
                _unitOfWork.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Handle_Should_Create_Client()
        {
            // Arrange
            var handler = CreateHandler();

            var command = new CreateClientCommand
            {
                Name = "Juan Perez",
                Sex = "M" 
            };

            // Act
            var clientId = await handler.Handle(command, CancellationToken.None);

            // Assert
            clientId.ShouldNotBe(Guid.Empty);

            var client = _unitOfWork.Object.BankingSolutionDbContext.Clients!
                .FirstOrDefault(c => c.Id == clientId);

            client.ShouldNotBeNull();
            client!.Name.ShouldBe("Juan Perez");
            client.Sex.ShouldBe("M");
        }
    }
}
