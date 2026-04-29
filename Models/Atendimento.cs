namespace PIM_TechTrust.Models
{
    public class Atendimento
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public Usuario Cliente { get; set; }

        public int TecnicoId { get; set; }
        public Usuario Tecnico { get; set; }

        public string Descricao { get; set; }
        public string Status { get; set; }

        public DateTime DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }

    }
}
