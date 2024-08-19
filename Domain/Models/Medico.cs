using Application.Interfaces.Repositories;

namespace Domain.Models
{
    public class Medico : IEntity
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int CRM { get; set; }
        public string? Email { get; set; }

        public IList<Consulta> Consultas { get; set; } = new List<Consulta>();
        public IList<Agenda> Agendas { get; set; } = new List<Agenda>();
    }
}
