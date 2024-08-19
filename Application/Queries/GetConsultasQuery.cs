using Application.DTOs;
using AutoMapper;
using Infrastructure.Data.Repositories;
using MediatR;

namespace Application.Queries
{
    public class GetConsultasQuery : IRequest<List<ConsultaDto>>
    {
    }

    public class GetConsultasQueryHandler : IRequestHandler<GetConsultasQuery, List<ConsultaDto>>
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IMapper _mapper;

        public GetConsultasQueryHandler(IConsultaRepository consultaRepository, IMapper mapper)
        {
            _consultaRepository = consultaRepository;
            _mapper = mapper;
        }

        public async Task<List<ConsultaDto>> Handle(GetConsultasQuery request, CancellationToken cancellationToken)
        {
            var consultas = await _consultaRepository.GetConsultasFromDate(DateTime.UtcNow);
            var consultasDto = _mapper.Map<List<ConsultaDto>>(consultas);

            return consultasDto
                .OrderBy(c => c.Dia)
                .ThenBy(c => c.Horario)
                .ToList();
        }
    }

}