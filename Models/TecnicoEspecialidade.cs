namespace PIM_TechTrust.Models
{
    public class TecnicoEspecialidade
    {
        public int TecnicoPerfilId { get; set; }
        public TecnicoPerfil TecnicoPerfil { get; set; } = null!;

        public int EspecialidadeId { get; set; }
        public Especialidade Especialidade { get; set; } = null!;

    }
}
