namespace PIM_TechTrust.Models
{
    public class Favorito
    {
        public int ClienteId { get; set; }
        public Usuario Cliente { get; set; } = null!;

        public int TecnicoId { get; set; }
        public Usuario Tecnico { get; set; } = null!;
    }
}
