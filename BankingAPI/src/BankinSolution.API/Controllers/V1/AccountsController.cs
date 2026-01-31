using Asp.Versioning;
using BankingSolution.Application.Features.Accounts.Commands.CreateAccount;
using BankingSolution.Application.Features.Accounts.Commands.CreateClient;
using BankingSolution.Application.Features.Accounts.Commands.CreateDeposit;
using BankingSolution.Application.Features.Accounts.Commands.CreateWithdraw;
using BankingSolution.Application.Features.Accounts.Querys.GetBalanceByAccount;
using BankingSolution.Application.Features.Accounts.Querys.GetTransactionsByAccount;
using BankingSolution.Application.Features.Accounts.Querys.Vms;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BankinSolution.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crea el perfil de un cliente en el sistema.
        /// </summary>
        /// <param name="command">Información básica del cliente.</param>
        /// <returns>Identificador del cliente creado.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        /// {
        ///   "name": "Juan Pérez",
        ///   "dateOfBirth": "1990-05-15",
        ///   "sex": "M",
        ///   "income": 2500.00
        /// }
        /// </remarks>
        [HttpPost("clients")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> CreateClient([FromBody] CreateClientCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Crea una cuenta bancaria asociada a un cliente existente.
        /// </summary>
        /// <param name="command">Datos necesarios para la creación de la cuenta.</param>
        /// <returns>Identificador de la cuenta creada.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        /// {
        ///   "clientId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "initialBalance": 1000.00
        /// }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> CreateAccount([FromBody] CreateAccountCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el saldo actual de una cuenta bancaria mediante su número de cuenta.
        /// </summary>
        /// <param name="accountNumber">Número de la cuenta bancaria.</param>
        /// <returns>Saldo actual de la cuenta.</returns>
        [HttpGet("{accountNumber}/balance")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<decimal>> GetBalance(string accountNumber)
        {
            var query = new GetBalanceByAccountQuery(accountNumber);
            var balance = await _mediator.Send(query);
            return Ok(balance);
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
