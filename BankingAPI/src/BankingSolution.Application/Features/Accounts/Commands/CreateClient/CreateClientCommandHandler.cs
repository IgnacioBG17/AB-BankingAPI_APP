using AutoMapper;
using BankingSolution.Application.Persistence;
using BankingSolution.Domain.Entities;
using MediatR;

namespace BankingSolution.Application.Features.Accounts.Commands.CreateClient
{
    public class CreateClientCommandHandler
         : IRequestHandler<CreateClientCommand, Guid>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateClientCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(
            CreateClientCommand request,
            CancellationToken cancellationToken)
        {
            var clientEntity = _mapper.Map<Client>(request);

            await _unitOfWork.Repository<Client>().AddAsync(clientEntity);
            await _unitOfWork.Complete();

            return clientEntity.Id;
        }
    }
}
