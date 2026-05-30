using PIM_TechTrust.Models;

namespace PIMIII_CLICKTECK.Models.ViewModels
{
    public class PerfilTecnicoViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }

        public string? FotoPerfil { get; set; }

        public bool Disponivel { get; set; }

        public IFormFile? NovaFoto { get; set; }

        public List<int> EspecialidadesSelecionadas { get; set; } = new();

        public List<Especialidade> TodasEspecialidades { get; set; } = new();

    }
}
