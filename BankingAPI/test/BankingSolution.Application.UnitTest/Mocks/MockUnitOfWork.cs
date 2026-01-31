using BankingSolution.Infrastructure.Persistence;
using BankingSolution.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BankingSolution.Application.UnitTest.Mocks
{
    public class MockUnitOfWork
    {
        public static Mock<UnitOfWork> GetUnitOfWork()
        {
            Guid dbContextId = Guid.NewGuid();
            var options = new DbContextOptionsBuilder<BankingSolutionDbContext>()
                .UseInMemoryDatabase(databaseName: $"StreamerDbContext-{dbContextId}")
                .Options;

            var streamerDbContextFake = new BankingSolutionDbContext(options);
            streamerDbContextFake.Database.EnsureDeleted();
            var mockUnitOfWork = new Mock<UnitOfWork>(streamerDbContextFake);

            return mockUnitOfWork;
        }
    }
}
