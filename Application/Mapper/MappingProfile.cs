using Application.DTOs;
using AutoMapper;
using Domain.Models;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Consulta, ConsultaDto>()
                .ForMember(dest => dest.Dia, opt => opt.MapFrom(src => src.Dia.ToString("yyyy-MM-dd")));

            CreateMap<Medico, MedicoDto>();

            CreateMap<Agenda, AgendaDto>()
                .ForMember(dest => dest.Dia, opt => opt.MapFrom(src => src.Dia.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Horarios, opt => opt.MapFrom(src => src.Horarios.Select(h => h).ToList()));
        }
    }
}