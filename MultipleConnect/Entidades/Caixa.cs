using MultipleConnect.Enuns;

namespace MultipleConnect.Entidades
{
    public class Caixa : Base
    {
        public Caixa()
        {
        }

        public long UserId { get; set; }

        public long EmpresaId { get; set; }

        public long? FormaPagamentoId { get; set; }

        public int NumeroParcela { get; set; }
        public long? ChequeId { get; set; }

        public long? ClienteId { get; set; }
        public long? PedidoId { get; set; }

        public long? CentroCustoId { get; set; }

        public long? ContaId { get; set; }

        public string ContaNome { get; set; }

        public long? ContaOrigemId { get; set; }
        public long? ContaDestinoId { get; set; }

        public TipoMovimento Operacao { get; set; }
        public bool? Estorno { get; set; }
        public bool? Transferencia { get; set; }

        public long Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public decimal ValorTotal { get; set; }

        public decimal SaldoConta { get; set; }
        public decimal SaldoCaixaAnterior { get; set; }
        public decimal SaldoCaixa { get; set; }

        public string? Obs { get; set; }
    }
}
