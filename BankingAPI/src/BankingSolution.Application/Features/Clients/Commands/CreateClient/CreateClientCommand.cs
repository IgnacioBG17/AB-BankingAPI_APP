using MediatR;

namespace BankingSolution.Application.Features.Clients.Commands.CreateClient
{
    public class CreateClientCommand : IRequest<Guid>
    {
        public string Name { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; } = null!;
        public decimal Income { get; set; }
    }
}
