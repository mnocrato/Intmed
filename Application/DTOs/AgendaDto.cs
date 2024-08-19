namespace Application.DTOs
{
    public class AgendaDto
    {
        public int Id { get; set; }
        public MedicoDto Medico { get; set; }
        public string Dia { get; set; }
        public List<string> Horarios { get; set; }
    }
}