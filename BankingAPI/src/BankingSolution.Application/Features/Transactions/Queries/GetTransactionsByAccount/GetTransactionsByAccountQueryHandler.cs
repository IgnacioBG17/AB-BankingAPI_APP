using AutoMapper;
using BankingSolution.Application.Exceptions;
using BankingSolution.Application.Features.Transactions.Queries.Vms;
using BankingSolution.Application.Persistence;
using BankingSolution.Domain.Entities;
using MediatR;

namespace BankingSolution.Application.Features.Transactions.Queries.GetTransactionsByAccount
{
    public class GetTransactionsByAccountQueryHandler
         : IRequestHandler<GetTransactionsByAccountQuery, AccountStatementVm>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTransactionsByAccountQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AccountStatementVm> Handle(
            GetTransactionsByAccountQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Validar existencia de la cuenta
            var account = await _unitOfWork.Repository<BankAccount>()
                .GetEntityAsync(a => a.AccountNumber == request.AccountNumber);

            if (account is null)
                throw new BadRequestException("Cuenta no encontrada");

            // 2. Obtener TODAS las transacciones ordenadas
            var transactions = await _unitOfWork.Repository<Transaction>().GetAsync(
                t => t.BankAccountId == account.Id,
                orderBy: q => q.OrderBy(t => t.CreatedAt),
                includes: null,
                disableTracking: true
            );

            var transactionList = _mapper.Map<List<TransactionVm>>(transactions);

            return new AccountStatementVm
            {
                AccountNumber = account.AccountNumber,
                FinalBalance = account.Balance,
                Transactions = transactionList
            };
        }
    }
}
