using AutoMapper;
using BankingSolution.Application.Exceptions;
using BankingSolution.Application.Features.Transactions.Queries.Vms;
using BankingSolution.Application.Persistence;
using BankingSolution.Domain.Entities;
using BankingSolution.Domain.Enum;
using MediatR;

namespace BankingSolution.Application.Features.Transactions.Commands.CreateWithdraw
{
    public class WithdrawCommandHandler
         : IRequestHandler<WithdrawCommand, TransactionVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WithdrawCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TransactionVm> Handle(
            WithdrawCommand request,
            CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Repository<BankAccount>()
                .GetEntityAsync(a => a.AccountNumber == request.AccountNumber);

            if (account is null)
                throw new NotFoundException(nameof(BankAccount), request.AccountNumber!);

            // Validar fondos
            if (account.Balance < request.Amount)
                throw new BadRequestException("Fondos insuficientes.");

            // Disminuir balance
            account.Balance -= request.Amount;

            var transaction = new Transaction
            {
                BankAccountId = account.Id,
                Type = TransactionType.Withdrawal,
                Amount = request.Amount,
                BalanceAfter = account.Balance,
                CreatedAt = DateTime.UtcNow,
                Description = request.Description
            };

            _unitOfWork.Repository<Transaction>().AddEntity(transaction);
            _unitOfWork.Repository<BankAccount>().UpdateEntity(account);

            await _unitOfWork.Complete();

            return _mapper.Map<TransactionVm>(transaction);
        }
    }
}
