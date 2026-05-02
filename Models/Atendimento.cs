namespace PIM_TechTrust.Models
{
    public class Atendimento
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public Usuario Cliente { get; set; }

        public int TecnicoId { get; set; }
        public Usuario Tecnico { get; set; }

        /*A interrogação indica que e um tipo nullable, que pode ser criado sem
        essas informações e depois ser adicionado; Ou seja, o cliente vai criar 
        e depois o técnico vai atualizar esses campos.
         */
        public decimal? ValorOrcamento { get; set; }
        public string? ObservacaoTecnico { get; set; }

        public string Descricao { get; set; } //Problema relatado pelo cliente

        
        public string Status { get; set; } // Solicitado / Pendente Aprovação / Aprovado / Recusado / Finalizado

        public DateTime DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }

    }
}
