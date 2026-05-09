namespace PIMIII_CLICKTECK.Models.ViewModels
{
    public class DashBoardViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string FotoUrl { get; set; }
        public string Descricao { get; set; }
        public double Avaliacao { get; set; }
        public int QtdReparos { get; set; }
        public List<string> Tags { get; set; }
    }
}
