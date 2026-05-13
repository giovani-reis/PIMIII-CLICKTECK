using System.ComponentModel.DataAnnotations;

namespace PIM_TechTrust.Models.Enums
{
    public enum StatusAtendimento 
    {
        Solicitado,
        [Display(Name = "Pendente Aprovação")]
        PendenteAprovacao,
        Aprovado,
        Cancelado,
        Finalizado
    }
}
