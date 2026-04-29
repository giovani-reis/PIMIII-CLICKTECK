namespace PIM_TechTrust.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public Usuario Cliente { get; set; }

        public int TecnicoId { get; set; }
        public Usuario Tecnico { get; set; }

        public int Nota { get; set; }
        public string? Comentario { get; set; }
    }
}
