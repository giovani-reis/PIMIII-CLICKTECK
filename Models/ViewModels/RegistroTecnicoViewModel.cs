namespace PIMIII_CLICKTECK.Models.ViewModels
{
    public class RegistroTecnicoViewModel
    {
        // Dados para a tabela Usuario
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        // Dados para a tabela TecnicoPerfil
        public string? Descricao { get; set; }
        public IFormFile? Foto { get; set; } // O "carregador" da foto que discutimos

        // Dados para a tabela associativa
        public List<int> EspecialidadesIds { get; set; } = new();
    }
}
