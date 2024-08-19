using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;
public class MedicoRepository : Repository<Medico>, IMedicoRepository
{
    public MedicoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Medico> GetByCRMAsync(int crm)
        => await _context.Medicos.FirstOrDefaultAsync(m => m.CRM == crm);
}