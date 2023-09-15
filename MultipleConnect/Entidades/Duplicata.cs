using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultipleConnect.Enuns;

namespace MultipleConnect.Entidades
{
    public class Duplicata : Base
    {
        public Duplicata()
        {
            Pagamentos = new List<Pagamento>();
        }

        public long FaturaId { get; set; }

        public int NumeroParcela { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorParcela { get; set; }

        public DateTime Vencimento { get; set; }

        public TipoDuplicataStatus Status { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Desconto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Acrescimo { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorDesconto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorAcrescimo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorPagamento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotalPagamento { get; set; }

        public List<Pagamento> Pagamentos { get; set; }
    }
}
