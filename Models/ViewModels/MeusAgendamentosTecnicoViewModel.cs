namespace PIMIII_CLICKTECK.Models.ViewModels
{
    public class MeusAgendamentosTecnicoViewModel
    {
        public int AtendimentoId { get; set; }
        public string ClienteNome { get; set; }
        public string Aparelho { get; set; }
        public string Descricao { get; set; }
        public decimal? ValorOrcamento { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string Status { get; set; }
    }
}
