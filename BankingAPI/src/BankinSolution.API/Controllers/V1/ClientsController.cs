using Asp.Versioning;
using BankingSolution.Application.Features.Clients.Commands.CreateClient;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BankinSolution.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClientsController(IMediator mediator)
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
    }
}
