    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIM_TechTrust.Models
{
    public class TecnicoPerfil
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public string? Descricao { get; set; }
        public string? FotoUrl { get; set; }
        [NotMapped]
        public IFormFile? FotoFile { get; set; }
        public bool Disponivel { get; set; }

        public ICollection<TecnicoEspecialidade> Especialidades { get; set; }
    }
}
