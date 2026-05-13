namespace PIMIII_CLICKTECK.Models.ViewModels
{
    public class MeusAgendamentosViewModel
    {
        public int AtendimentoId { get; set; }
        public string TecnicoNome { get; set; }
        public string TecnicoEspecialidade { get; set; }
        public string TecnicoFoto { get; set; }
        public string ServicoDescricao { get; set; }
        public DateTime DataAbertura { get; set; }
        public string Status { get; set; }
    }
}
