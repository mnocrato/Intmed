using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class ConsultaRepository : Repository<Consulta>, IConsultaRepository
{
    public ConsultaRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Consulta>> GetConsultasFromDate(DateTime date)
        => await _context.Consultas.Where(c => c.Dia >= date).Include(i => i.Medico).ToListAsync();

    public async Task<Consulta> GetConsultaByIdWithMedico(int id)
        => await _context.Consultas.Where(c => c.Id >= id).Include(i => i.Medico).FirstOrDefaultAsync();
}
