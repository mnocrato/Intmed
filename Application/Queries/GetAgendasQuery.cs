using Application.DTOs;
using AutoMapper;
using Infrastructure.Data.Repositories;
using MediatR;

namespace Application.Queries
{
    public class GetAgendasQuery : IRequest<List<AgendaDto>>
    {
        public List<int> MedicoIds { get; set; }
        public List<int> CrmIds { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }
    }

    public class GetAgendasQueryHandler : IRequestHandler<GetAgendasQuery, List<AgendaDto>>
    {
        private readonly IAgendaRepository _agendaRepository;
        private readonly IMapper _mapper;

        public GetAgendasQueryHandler(IAgendaRepository agendaRepository, IMapper mapper)
        {
            _agendaRepository = agendaRepository;
            _mapper = mapper;
        }

        public async Task<List<AgendaDto>> Handle(GetAgendasQuery request, CancellationToken cancellationToken)
        {
            var agendas = await _agendaRepository.GetAvailableAgendasAsync(request.MedicoIds, request.CrmIds, request.DataInicio, request.DataFinal);
            var agendasDto = _mapper.Map<List<AgendaDto>>(agendas);

            return agendasDto
                .Select(a => new AgendaDto
                {
                    Id = a.Id,
                    Medico = a.Medico,
                    Dia = a.Dia,
                    Horarios = a.Horarios.Where(h => DateTime.Parse(h) >= DateTime.UtcNow).ToList()
                })
                .OrderBy(a => a.Dia)
                .ToList();
        }
    }

}