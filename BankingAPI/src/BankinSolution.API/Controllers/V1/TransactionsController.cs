using Asp.Versioning;
using BankingSolution.Application.Features.Transactions.Commands.CreateDeposit;
using BankingSolution.Application.Features.Transactions.Commands.CreateWithdraw;
using BankingSolution.Application.Features.Transactions.Queries.GetTransactionsByAccount;
using BankingSolution.Application.Features.Transactions.Queries.Vms;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BankinSolution.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Registra un depósito en una cuenta bancaria e incrementa su saldo.
        /// </summary>
        /// <param name="accountNumber">Número de la cuenta bancaria.</param>
        /// <param name="command">Información del depósito.</param>
        /// <returns>Detalle de la transacción realizada.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        /// {
        ///   "accountNumber": "CR-1001",
        ///   "amount": 500.00,
        ///   "description": "Depósito en efectivo"
        /// }
        /// </remarks>
        [HttpPost("deposit")]
        [ProducesResponseType(typeof(TransactionVm), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionVm>> Deposit(
           [FromBody] DepositCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Registra un retiro de una cuenta bancaria validando fondos suficientes.
        /// </summary>
        /// <param name="accountNumber">Número de la cuenta bancaria.</param>
        /// <param name="command">Información del retiro.</param>
        /// <returns>Detalle de la transacción realizada.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///{
        ///  "accountNumber": "CR-1001",
        ///  "amount": 1000.00,
        ///  "description": "Retiro en cajero"
        ///}
        /// </remarks>
        [HttpPost("withdraw")]
        [ProducesResponseType(typeof(TransactionVm), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionVm>> Withdraw(
            [FromBody] WithdrawCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Obtiene el estado de cuenta que incluye el saldo final calculado y el historial 
        /// detallado de transacciones (depósitos y retiros).
        /// </summary>
        /// <param name="accountNumber">Número de la cuenta bancaria única (ej. CR-1001).</param>
        /// <returns>Un objeto con el saldo final y el listado de transacciones.</returns>
        [HttpGet("{accountNumber}/transactions")]
        [ProducesResponseType(typeof(AccountStatementVm), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountStatementVm>> GetTransactions(string accountNumber)
        {
            var query = new GetTransactionsByAccountQuery(accountNumber);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
