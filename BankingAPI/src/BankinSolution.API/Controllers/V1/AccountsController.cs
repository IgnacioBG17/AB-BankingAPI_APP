using Asp.Versioning;
using BankingSolution.Application.Features.Accounts.Commands.CreateAccount;
using BankingSolution.Application.Features.Accounts.Queries.GetBalanceByAccount;
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
    }
}
