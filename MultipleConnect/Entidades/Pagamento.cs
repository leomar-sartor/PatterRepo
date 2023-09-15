using MultipleConnect.Enuns;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultipleConnect.Entidades
{
    public class Pagamento : Base
    {
        public Pagamento()
        {

        }

        public long UserId { get; set; }

        public long EmpresaId { get; set; }

        public long DuplicataId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Desconto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Acrescimo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        public string? Observacao { get; set; }

        public TipoPagamentoStatus Status { get; set; }
    }
}
