using AutoMapper;
using BankingSolution.Application.Exceptions;
using BankingSolution.Application.Persistence;
using BankingSolution.Domain.Entities;
using BankingSolution.Domain.Enum;
using MediatR;

namespace BankingSolution.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandHandler
         : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAccountCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(
            CreateAccountCommand request,
            CancellationToken cancellationToken)
        {
            // Validar cliente 
            var client = await _unitOfWork.Repository<Client>()
                .GetEntityAsync(c => c.Id == request.ClientId);

            if (client is null)
                throw new BadRequestException("El cliente no existe");

            // Crear cuenta
            var account = _mapper.Map<BankAccount>(request);
            account.AccountNumber = GenerateAccountNumber();
            account.Balance = request.InitialBalance;
            account.ClientId = request.ClientId;

            _unitOfWork.Repository<BankAccount>().AddEntity(account);

            // Transacción inicial
            if (request.InitialBalance > 0)
            {
                var transaction = new Transaction
                {
                    BankAccountId = account.Id,
                    Type = TransactionType.Deposit,
                    Amount = request.InitialBalance,
                    BalanceAfter = request.InitialBalance,
                    CreatedAt = DateTime.UtcNow,
                    Description = "Depósito inicial"
                };

                _unitOfWork.Repository<Transaction>().AddEntity(transaction);
            }

            await _unitOfWork.Complete();

            return account.Id;
        }

        private static string GenerateAccountNumber()
        {
            return $"ACC-{DateTime.UtcNow.Ticks}";
        }
    }
}
