using PIM_TechTrust.Models;

namespace PIMIII_CLICKTECK.Models.ViewModels
{
    public class DashboardTecnicoViewModel
    {
        public Usuario Tecnico { get; set; }
        public List<Atendimento> AtendimentosPendentes { get; set; }

        public decimal GanhosTotais { get; set; }
        public int TotalReparos { get; set; }
        public double AvaliacaoMedia { get; set; }
    }
}
