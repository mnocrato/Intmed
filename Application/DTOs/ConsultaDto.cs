namespace Application.DTOs
{
    public class ConsultaDto
    {
        public int Id { get; set; }
        public string Dia { get; set; }
        public string Horario { get; set; }
        public DateTime DataAgendamento { get; set; }
        public MedicoDto Medico { get; set; }
    }
}