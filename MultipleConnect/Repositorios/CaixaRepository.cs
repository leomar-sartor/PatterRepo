using Dapper;
using MultipleConnect.Entidades;
using System.Data;

namespace MultipleConnect.Repositorios
{
    public class CaixaRepository : Repository, IRepositorio<Caixa>
    {
        private string _select =
            @"SELECT 
	                C.Id,
                    C.UserId,
                    C.EmpresaId,
                    C.DataCriacao,
                    C.DataAlteracao,
                    C.Deletado,
                    C.FormaPagamentoId,
                    C.NumeroParcela,
                    C.ChequeId,
                    C.ClienteId,
                    C.CentroCustoId,
                    C.ContaId,
                    C2.Nome ContaNome,
                    C.ContaOrigemId,
                    C.ContaDestinoId,
                    C.PedidoId,
                    C.Operacao,
                    C.Estorno,
                    C.Transferencia,
                    C.Quantidade,
                    C.ValorUnitario,
                    C.ValorTotal,
                    C.SaldoConta,
                    C.SaldoCaixaAnterior,
                    C.SaldoCaixa,
                    C.Obs
                FROM 
	                Caixa C
                INNER JOIN Conta C2 ON C2.Id = C.ContaId
                ";
        public CaixaRepository(IConexao conexao) : base(conexao) => _buscarTodosTemplate = _select;

        //Inserir
        public long Inserir(Caixa dominio, IDbTransaction? transaction = null)
        {
            return Insert("Caixa", new
            {
                dominio.Id,
                dominio.UserId,
                dominio.EmpresaId,
                dominio.DataCriacao,
                dominio.DataAlteracao,
                dominio.Deletado,
                dominio.FormaPagamentoId,
                dominio.NumeroParcela,
                dominio.ChequeId,
                dominio.ClienteId,
                dominio.PedidoId,
                dominio.CentroCustoId,
                dominio.ContaId,
                dominio.ContaOrigemId,
                dominio.ContaDestinoId,
                dominio.Operacao,
                dominio.Estorno,
                dominio.Transferencia,
                dominio.Quantidade,
                dominio.ValorUnitario,
                dominio.ValorTotal,
                dominio.SaldoConta,
                dominio.SaldoCaixaAnterior,
                dominio.SaldoCaixa,
                dominio.Obs
            }, transaction);
        }

        //Alterar
        public long Alterar(Caixa dominio, IDbTransaction? transaction = null)
        {
            Update("Caixa", new
            {
                dominio.Id,
                dominio.UserId,
                dominio.EmpresaId,
                dominio.DataCriacao,
                dominio.DataAlteracao,
                dominio.Deletado,
                dominio.FormaPagamentoId,
                dominio.NumeroParcela,
                dominio.ChequeId,
                dominio.ClienteId,
                dominio.PedidoId,
                dominio.CentroCustoId,
                dominio.ContaId,
                dominio.ContaOrigemId,
                dominio.ContaDestinoId,
                dominio.Operacao,
                dominio.Estorno,
                dominio.Transferencia,
                dominio.Quantidade,
                dominio.ValorUnitario,
                dominio.ValorTotal,
                dominio.SaldoConta,
                dominio.SaldoCaixaAnterior,
                dominio.SaldoCaixa,
                dominio.Obs
            }, transaction);

            return dominio.Id;
        }

        //Salvar
        public long Salvar(Caixa dominio, IDbTransaction? transaction = null)
        {
            var obj = Buscar(dominio, transaction);
            if (obj == null)
                return Inserir(dominio, transaction);
            else
                return Alterar(dominio, transaction);
        }

        //Buscar
        public IEnumerable<Caixa> BuscarTodos(IDbTransaction? transaction = null)
        {
            return _conexao.Query<Caixa>(_select, null, transaction);
        }

        public IEnumerable<Caixa> BuscarTodosFilter(SqlBuilder sqlBuilder, int paginaAtual, out int totalLinhas)
        {
            var sql = sqlBuilder.AddTemplate($"{_select} /**where**/");

            return _conexao.Query<Caixa>(sql.RawSql, paginaAtual, out totalLinhas, sql.Parameters);
        }

        public Caixa Buscar(long Id, IDbTransaction? transaction = null)
        {
            var sqlBuscar = $"{_select} Where C.Id = @Id AND C.Deletado = 0";
            return _conexao.QueryFirstOrDefault<Caixa>(sqlBuscar, new { Id }, transaction);
        }

        public Caixa Buscar(Caixa dominio, IDbTransaction? transaction = null)
        {
            return Buscar(dominio.Id, transaction);
        }

        //Excluir
        public void Excluir(long Id, IDbTransaction? transaction = null)
        {
            Delete("Caixa", new { Id }, transaction);
        }

        public void Excluir(Caixa dominio, IDbTransaction? transaction = null)
        {
            Excluir(dominio, transaction);
        }

        //Adicionais
        public Caixa BuscarUltimo(IDbTransaction? transaction = null)
        {
            var sql = $"{_select} ORDER BY C.Id DESC LIMIT 1";
            return _conexao.QueryFirstOrDefault<Caixa>(sql, null, transaction);
        }

        public Caixa BuscarUltimoPorConta(long ContaId, IDbTransaction? transaction = null)
        {
            var sql = $"{_select}  Where C.ContaId = @ContaId ORDER BY C.Id DESC LIMIT 1";
            return _conexao.QueryFirstOrDefault<Caixa>(sql, new { ContaId }, transaction);
        }

        public IEnumerable<Caixa> BuscarPorPedido(long PedidoId, IDbTransaction? transaction = null)
        {
            var sql = $"{_select} Where C.PedidoId = @PedidoId AND C.Deletado = 0";
            return _conexao.Query<Caixa>(sql, new { PedidoId }, transaction);
        }
    }
}
