using BankingSolution.Application.Exceptions;
using BankingSolution.Application.Persistence;
using BankingSolution.Domain.Entities;
using MediatR;

namespace BankingSolution.Application.Features.Accounts.Queries.GetBalanceByAccount
{
    public class GetBalanceByAccountQueryHandler
          : IRequestHandler<GetBalanceByAccountQuery, decimal>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBalanceByAccountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<decimal> Handle(
            GetBalanceByAccountQuery request,
            CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Repository<BankAccount>()
                .GetEntityAsync(a => a.AccountNumber == request.AccountNumber);

            if (account is null)
                throw new BadRequestException("Cuenta no encontrada");

            return account.Balance;
        }
    }
}
