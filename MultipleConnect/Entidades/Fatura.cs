using MultipleConnect.Enuns;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultipleConnect.Entidades
{
    public class Fatura : Base
    {
        public Fatura()
        {
            Status = TipoFaturaStatus.Aberta;
        }

        public long ClienteFornecedorId { get; set; }

        [Display(Name = "Cliente")]
        public string? ClienteFornecedorNome { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        public TipoFatura Tipo { get; set; }

        public long EmpresaId { get; set; }

        public long? PedidoId { get; set; }

        public long ContaId { get; set; }

        public long FormaPagamentoId { get; set; }

        public string? FormaPagamentoNome { get; set; }

        public TipoFaturaStatus Status { get; set; }

        public long CentroCustoId { get; set; }

        public string? Observacao { get; set; }

        public List<Duplicata> Duplicatas { get; set; }
    }
}
