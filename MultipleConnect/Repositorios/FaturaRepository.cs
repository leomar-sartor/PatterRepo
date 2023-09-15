using Dapper;
using MultipleConnect.Entidades;
using System.Data;

namespace MultipleConnect.Repositorios
{
    public class FaturaRepository : Repository, IRepositorio<Fatura>
    {
        private string _select =
            @"SELECT 
	                F.Id,
                    F.UserId,
                    F.EmpresaId,
                    F.DataCriacao,
                    F.DataAlteracao,
                    F.Deletado,
                    F.ClienteFornecedorId,
                    U.Nome ClienteFornecedorNome,
                    F.ValorTotal,
                    F.Tipo,
                    F.PedidoId,
                    F.FormaPagamentoId,
                    FP.Nome FormaPagamentoNome,
                    F.Status,
                    F.CentroCustoId,
                    F.Observacao
                FROM 
	                Fatura F
                INNER JOIN user U ON U.Id = F.ClienteFornecedorId 
	            INNER JOIN formapagamento FP ON FP.Id  = F.FormaPagamentoId
                ";
        public FaturaRepository(IConexao conexao) : base(conexao) => _buscarTodosTemplate = _select;

        //Inserir
        public long Inserir(Fatura dominio, IDbTransaction? transaction = null)
        {
            return Insert("Fatura", new
            {
                dominio.Id,
                dominio.UserId,
                dominio.EmpresaId,
                dominio.DataCriacao,
                dominio.DataAlteracao,
                dominio.Deletado,
                dominio.ClienteFornecedorId,
                dominio.ValorTotal,
                dominio.Tipo,
                dominio.PedidoId,
                dominio.FormaPagamentoId,
                dominio.Status,
                dominio.CentroCustoId,
                dominio.Observacao
            }, transaction);
        }

        //Alterar
        public long Alterar(Fatura dominio, IDbTransaction? transaction = null)
        {
            Update("Fatura", new
            {
                dominio.Id,
                dominio.UserId,
                dominio.EmpresaId,
                dominio.DataCriacao,
                dominio.DataAlteracao,
                dominio.Deletado,
                dominio.ClienteFornecedorId,
                dominio.ValorTotal,
                dominio.Tipo,
                dominio.PedidoId,
                dominio.FormaPagamentoId,
                dominio.Status,
                dominio.CentroCustoId,
                dominio.Observacao
            }, transaction);

            return dominio.Id;
        }

        //Salvar
        public long Salvar(Fatura dominio, IDbTransaction? transaction = null)
        {
            var obj = Buscar(dominio, transaction);
            if (obj == null)
                return Inserir(dominio, transaction);
            else
                return Alterar(dominio, transaction);
        }

        //Buscar
        public IEnumerable<Fatura> BuscarTodos(IDbTransaction? transaction = null)
        {
            return _conexao.Query<Fatura>(_select, null, transaction);
        }

        public IEnumerable<Fatura> BuscarTodosFilter(SqlBuilder sqlBuilder, int paginaAtual, out int totalLinhas)
        {
            var sql = sqlBuilder.AddTemplate($"{_select} /**where**/");

            return _conexao.Query<Fatura>(sql.RawSql, paginaAtual, out totalLinhas, sql.Parameters);
        }

        public Fatura Buscar(long Id, IDbTransaction? transaction = null)
        {
            var sqlBuscar = $"{_select} Where F.Id = @Id AND F.Deletado = 0";
            return _conexao.QueryFirstOrDefault<Fatura>(sqlBuscar, new { Id }, transaction);
        }

        public Fatura Buscar(Fatura dominio, IDbTransaction? transaction = null)
        {
            return Buscar(dominio.Id, transaction);
        }

        //Excluir
        public void Excluir(long Id, IDbTransaction? transaction = null)
        {
            Delete("Caixa", new { Id }, transaction);
        }

        public void Excluir(Fatura dominio, IDbTransaction? transaction = null)
        {
            Excluir(dominio, transaction);
        }

        //Adicionais
        public Fatura BuscarPorPedido(long PedidoId, IDbTransaction transaction)
        {
            var sql = $"{_select} Where F.PedidoId = @PedidoId AND F.Deletado = 0";
            return _conexao.QueryFirstOrDefault<Fatura>(sql, new { PedidoId }, transaction);
        }
    }
}
