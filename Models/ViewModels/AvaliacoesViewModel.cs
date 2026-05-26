using PIM_TechTrust.Models;

namespace PIMIII_CLICKTECK.Models.ViewModels
{
    public class AvaliacoesViewModel
    {
        public double MediaNotas { get; set; }
        public int TotalAvaliacoes { get; set; }

        public int Quantidade5 { get; set; }
        public int Quantidade4 { get; set; }
        public int Quantidade3 { get; set; }
        public int Quantidade2 { get; set; }
        public int Quantidade1 { get; set; }

        public List<Avaliacao> Avaliacoes { get; set; } = new();
    }
}
