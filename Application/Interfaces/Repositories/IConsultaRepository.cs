using Application.Interfaces.Repositories;
using Domain.Models;

namespace Infrastructure.Data.Repositories
{
    public interface IConsultaRepository : IRepository<Consulta>
    {
        Task<Consulta> GetConsultaByIdWithMedico(int id);
        Task<List<Consulta>> GetConsultasFromDate(DateTime date);
    }
}