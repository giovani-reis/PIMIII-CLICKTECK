using System.ComponentModel.DataAnnotations;

namespace PIM_TechTrust.Models
{
    public class Especialidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<TecnicoEspecialidade> Tecnicos { get; set; }
    }
}
