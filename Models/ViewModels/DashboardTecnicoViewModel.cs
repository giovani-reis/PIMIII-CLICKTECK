using PIM_TechTrust.Models;

namespace PIMIII_CLICKTECK.Models.ViewModels
{
    public class DashboardTecnicoViewModel
    {
        public Usuario Tecnico { get; set; }
        public List<Atendimento> AtendimentosPendentes { get; set; }
        // Card 1
        public decimal GanhosTotais { get; set; }
        public int TotalReparosConcluidos { get; set; }

        // Card 3
        public decimal AvaliacaoMedia { get; set; }
        public int TotalAvaliacoes { get; set; }


        public int TotalReparos { get; set; }
    }
}
