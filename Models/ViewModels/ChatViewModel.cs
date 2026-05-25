using PIM_TechTrust.Models;

namespace PIMIII_CLICKTECK.Models.ViewModels
{
    public class ChatViewModel
    {
        public int AtendimentoId { get; set; }

        public int UsuarioLogadoId { get; set; }

        public int OutroUsuarioId { get; set; }

        public string NomeOutroUsuario { get; set; }

        public IEnumerable<Mensagem> Mensagens { get; set; } = new List<Mensagem>();
            }
}
