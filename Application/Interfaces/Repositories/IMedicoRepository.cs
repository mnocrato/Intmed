using Application.Interfaces.Repositories;
using Domain.Models;

namespace Infrastructure.Data.Repositories
{
    public interface IMedicoRepository : IRepository<Medico>
    {
        Task<Medico> GetByCRMAsync(int crm);
    }
}