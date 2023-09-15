using MultipleConnect.Enuns;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultipleConnect.Entidades
{
    public class Pedido : Base
    {
        public Pedido()
        {
            Status = TipoStatusPedido.Aberto;
        }

        public long EmpresaId { get; set; }

        public long UserId { get; set; }

        public long ClienteId { get; set; }

        public long FormaPagamentoId { get; set; }

        public string FormaPagamentoNome { get; set; }

        public long LoteId { get; set; }

        public long LocalId { get; set; }

        public string LocalNome { get; set; }

        public string TalhaoNome { get; set; }

        public long Quantidade { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoPedido { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoFrete { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoTotalPedido { get; set; }

        public TipoStatusPedido Status { get; set; }

        public string? ClienteNome { get; set; }

        public string? LoteIdentificador { get; set; }

        public long ProdutoId { get; set; }

        public string? ProdutoNome { get; set; }

        public long GramaId { get; set; }

        public string? GramaNome { get; set; }

        public long? ResponsavelId { get; set; }

        public string ResponsavelNome { get; set; }

        public TipoSituacaoPedido? Situacao { get; set; }
    }
}
