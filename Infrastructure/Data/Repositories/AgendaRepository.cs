using Application.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class AgendaRepository : Repository<Agenda>, IAgendaRepository
{
    public AgendaRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Agenda> GetByMedicoAndDiaAsync(int medicoId, DateTime dia)
        => await _context.Agendas
            .Where(a => a.Medico.Id == medicoId && a.Dia.Date == dia.Date)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<AgendaDto>> GetAvailableAgendasAsync(List<int> medicoIds = null, List<int> crms = null, DateTime? dataInicio = null, DateTime? dataFinal = null)
    {
        var query = _context.Agendas
                .Include(a => a.Medico)
                .ThenInclude(a => a.Consultas).AsQueryable();

        if (medicoIds != null && medicoIds.Any())
            query = query.Where(a => medicoIds.Contains(a.Medico.Id));

        if (crms != null && crms.Any())
            query = query.Where(a => crms.Contains(a.Medico.CRM));

        if (dataInicio.HasValue)
            query = query.Where(a => a.Dia >= dataInicio.Value.Date);
        else
            query = query.Where(a => a.Dia >= DateTime.UtcNow);

        if (dataFinal.HasValue)
            query = query.Where(a => a.Dia <= dataFinal.Value.Date);

        var agendas = await query.ToListAsync();
        var agendasDTO = agendas
            .Select(a => new AgendaDto
            {
                Id = a.Id,
                Medico = new MedicoDto
                {
                    Id = a.Medico.Id,
                    Crm = a.Medico.CRM,
                    Nome = a.Medico.Nome,
                    Email = a.Medico.Email
                },
                Dia = a.Dia.ToString("yyyy-MM-dd"),
                Horarios = a.Horarios.Where(h => !a.Medico.Consultas.Any(c => c.Horario == h && c.Dia == a.Dia)).ToList()
            })
            .OrderBy(a => a.Dia)
            .ToList();

        return agendasDTO;
    }
}
