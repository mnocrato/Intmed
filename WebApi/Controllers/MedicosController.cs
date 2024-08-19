using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intmed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicosController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreateMedicoCommand command)
        {
            try
            {
                var medicoId = await _mediator.Send(command);
                return Created(nameof(Create), new { id = medicoId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}