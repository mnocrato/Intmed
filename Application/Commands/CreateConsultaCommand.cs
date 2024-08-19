using Application.DTOs;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Data.Repositories;
using MediatR;

namespace Application.Commands
{
    public class CreateConsultaCommand : IRequest<ConsultaDto>
    {
        public int AgendaId { get; set; }
        public string Horario { get; set; }
    }

    public class CreateConsultaCommandHandler : IRequestHandler<CreateConsultaCommand, ConsultaDto>
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IAgendaRepository _agendaRepository;

        public CreateConsultaCommandHandler(IConsultaRepository consultaRepository, IAgendaRepository agendaRepository)
        {
            _consultaRepository = consultaRepository;
            _agendaRepository = agendaRepository;
        }

        public async Task<ConsultaDto> Handle(CreateConsultaCommand request, CancellationToken cancellationToken)
        {
            var agenda = await _agendaRepository.GetByIdAsync(request.AgendaId) ??
                throw new NotFoundException("Consulta", "Agenda não encontrada.");

            if (agenda.Dia < DateTime.UtcNow.Date)
                throw new InvalidOperationException("Não é possível marcar consulta para um dia passado.");

            if (!agenda.Horarios.Contains(request.Horario))
                throw new InvalidOperationException("Horário não disponível.");

            var consulta = new Consulta
            {
                Id = request.AgendaId,
                Horario = request.Horario,
                DataAgendamento = DateTime.Now.ToUniversalTime(),
                Dia = agenda.Dia.ToUniversalTime(),
                MedicoId = agenda.MedicoId
            };

            var id = await _consultaRepository.AddAsync(consulta);

            var newConsulta = await _consultaRepository.GetConsultaByIdWithMedico(id);

            return new ConsultaDto
            {
                Id = newConsulta.Id,
                Dia = newConsulta.Dia.ToString("yyyy-MM-dd"),
                Horario = newConsulta.Horario,
                DataAgendamento = newConsulta.DataAgendamento,
                Medico = new MedicoDto
                {
                    Id = agenda.Medico.Id,
                    Crm = agenda.Medico.CRM,
                    Nome = agenda.Medico.Nome,
                    Email = agenda.Medico.Email
                }
            };
        }
    }

}