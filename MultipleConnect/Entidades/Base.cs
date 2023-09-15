using System.ComponentModel.DataAnnotations;

namespace MultipleConnect.Entidades
{
    public class Base
    {
        public Base()
        {
            DataCriacao = DateTime.Today;
        }

        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool Deletado { get; set; } = false;
    }
}
