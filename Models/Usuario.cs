using PIM_TechTrust.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PIM_TechTrust.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public Role Role { get; set; }


        public TecnicoPerfil? TecnicoPerfil { get; set; }


        public bool EhTecnico()
        {
            return true;

        }
        public bool EhCliente()
        {
            return true;
        }
        public bool EhAdmin()
        {
            return true;
        }

        public bool PodeAvaliar()
        {
            return true;
        }

        public bool PodeEnviarMensagem()
        {
            return true;
        }

    }
}
