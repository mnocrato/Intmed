using Application.Interfaces.Repositories;

namespace Domain.Models
{
    public class Agenda : IEntity
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public DateTime Dia { get; set; }
        public List<string> Horarios { get; set; } = new List<string>();

        public Medico Medico { get; set; }
    }
}
