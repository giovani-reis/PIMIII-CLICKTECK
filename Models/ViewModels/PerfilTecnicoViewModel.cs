using PIM_TechTrust.Models;

namespace PIMIII_CLICKTECK.Models.ViewModels
{
    public class PerfilTecnicoViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string FotoPerfil { get; set; }
        public IEnumerable<string> Especialidades { get; set; } = new List<string>();
    }
}
