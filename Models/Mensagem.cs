namespace PIM_TechTrust.Models
{
    public class Mensagem
    {
        public int Id { get; set; }

        public int RemetenteId { get; set; }
        public Usuario Remetente { get; set; }

        public int DestinatarioId { get; set; }
        public Usuario Destinatario { get; set; }

        public string Conteudo { get; set; }
        public DateTime DataEnvio { get; set; } = DateTime.Now;
    }
}
