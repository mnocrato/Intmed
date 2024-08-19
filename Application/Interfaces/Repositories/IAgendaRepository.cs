using Application.DTOs;
using Application.Interfaces.Repositories;
using Domain.Models;

namespace Infrastructure.Data.Repositories
{
    public interface IAgendaRepository : IRepository<Agenda>
    {
        Task<IEnumerable<AgendaDto>> GetAvailableAgendasAsync(List<int> medicoIds = null, List<int> crms = null, DateTime? dataInicio = null, DateTime? dataFinal = null);
        Task<Agenda> GetByMedicoAndDiaAsync(int medicoId, DateTime dia);
    }
}