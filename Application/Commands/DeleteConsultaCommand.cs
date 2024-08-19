using Domain.Exceptions;
using Infrastructure.Data.Repositories;
using MediatR;

namespace Application.Commands
{
    public record DeleteConsultaCommand(int Id) : IRequest;

    public class DeleteConsultaCommandHandler : IRequestHandler<DeleteConsultaCommand>
    {
        private readonly IConsultaRepository _consultaRepository;

        public DeleteConsultaCommandHandler(IConsultaRepository consultaRepository)
            => _consultaRepository = consultaRepository;

        public async Task<Unit> Handle(DeleteConsultaCommand request, CancellationToken cancellationToken)
        {
            var consulta = await _consultaRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException("Consulta", "Consulta não encontrada.");

            if (consulta.Dia < DateTime.UtcNow.Date)
                throw new InvalidOperationException("Não é possível desmarcar uma consulta que já aconteceu.");

            await _consultaRepository.DeleteAsync(consulta.Id);

            return Unit.Value;
        }
    }
}