using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Data.Repositories;
using MediatR;

namespace Application.Commands
{
    public class CreateAgendaCommand : IRequest<int>
    {
        public int MedicoId { get; set; }
        public DateTime Dia { get; set; }
        public List<string> Horarios { get; set; }
    }

    public class CreateAgendaCommandHandler : IRequestHandler<CreateAgendaCommand, int>
    {
        private readonly IAgendaRepository _agendaRepository;
        private readonly IMedicoRepository _medicoRepository;

        public CreateAgendaCommandHandler(IAgendaRepository agendaRepository, IMedicoRepository medicoRepository)
        {
            _agendaRepository = agendaRepository;
            _medicoRepository = medicoRepository;
        }

        public async Task<int> Handle(CreateAgendaCommand request, CancellationToken cancellationToken)
        {
            var medico = await _medicoRepository.GetByIdAsync(request.MedicoId)
                ?? throw new NotFoundException(nameof(Medico), request.MedicoId);

            var existingAgenda = await _agendaRepository.GetByMedicoAndDiaAsync(request.MedicoId, request.Dia);
            if (existingAgenda != null)
                throw new ConflictException("Já existe uma agenda para este médico no mesmo dia.");

            if (request.Dia.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("Não é possível criar uma agenda para um dia passado.");
            
            if(!IsValidTimeFormat(request.Horarios))
                throw new ArgumentException("Horario informado em formato não aceitavel.");

            var agenda = new Agenda
            {
                Medico = medico,
                Dia = request.Dia.ToUniversalTime(),
                Horarios = request.Horarios
            };

            return await _agendaRepository.AddAsync(agenda);
        }

        public static bool IsValidTimeFormat(List<string> timeString)
        {
            return timeString.All(s => TimeSpan.TryParseExact(s, "hh\\:mm", null, out _));
        }
    }
}