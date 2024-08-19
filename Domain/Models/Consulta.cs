using Application.Interfaces.Repositories;

namespace Domain.Models
{
    public class Consulta : IEntity
    {
        public int Id { get; set; }
        public DateTime Dia { get; set; }
        public string Horario { get; set; }
        public DateTime DataAgendamento { get; set; }
        public int MedicoId { get; set; }
        public Medico Medico { get; set; }
    }
}
