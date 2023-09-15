using MultipleConnect.Enuns;

namespace MultipleConnect.Entidades
{
    public class Movimento : Base
    {
        public Movimento()
        {
            Ignorar = false;
        }

        public long? PedidoId { get; set; }

        public long EmpresaId { get; set; }

        public long UserId { get; set; }
        public long LoteId { get; set; }
        public string Identificador { get; set; }
        public long TalhaoId { get; set; }
        public long GramaId { get; set; }
        public string TalhaoNome { get; set; }

        public long TipoProdutoId { get; set; }
        public string TipoProdutoNome { get; set; }

        public long LocalId { get; set; }
        public string LocalNome { get; set; }

        public long? ClienteId { get; set; }
        public string? ClienteNome { get; set; }

        public long IntervaloDeCorte { get; set; }
        public long Quantidade { get; set; }
        public long SaldoAnterior { get; set; }

        public bool? Estorno { get; set; }

        public bool? Perca { get; set; }

        public bool? Transferencia { get; set; }

        public bool? Ignorar { get; set; }

        public TipoMovimento Operacao { get; set; }
        public long Saldo { get; set; }

        public string? Obs { get; set; }
    }
}
