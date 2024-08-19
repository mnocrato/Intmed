using Application.Commands;
using Application.Queries;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intmed.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AgendasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AgendasController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAgendaCommand command)
        {
            try
            {
                var agendaId = await _mediator.Send(command);
                return Created(nameof(Create), new { id = agendaId });
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] List<int> medico, [FromQuery] List<int> crm, [FromQuery] DateTime? data_inicio, [FromQuery] DateTime? data_final)
        {
            var query = new GetAgendasQuery
            {
                MedicoIds = medico,
                CrmIds = crm,
                DataInicio = data_inicio,
                DataFinal = data_final
            };

            var agendas = await _mediator.Send(query);
            return Ok(agendas);
        }
    }
}