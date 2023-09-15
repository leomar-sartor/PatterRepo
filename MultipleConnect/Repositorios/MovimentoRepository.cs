using Dapper;
using MultipleConnect.Entidades;
using System.Data;

namespace MultipleConnect.Repositorios
{
    public class MovimentoRepository : Repository, IRepositorio<Movimento>
    {
        private string _select =
            @"SELECT 
		            M.Id,
                    M.EmpresaId,
                    M.PedidoId,
		            M.LoteId,
                    L.Identificador,
		            M.TalhaoId,
                    M.LocalId,
                    M.GramaId,
                    TP.Descricao LocalNome,
                    T.Nome TalhaoNome,
                    L.TipoProdutoId TipoProdutoId,
                    P.Descricao TipoProdutoNome,
                    M.ClienteId,
                    C.Nome ClienteNome,
		            M.Quantidade,
		            M.Operacao,
		            M.DataCriacao,
		            M.DataAlteracao,
		            M.Deletado,
                    M.SaldoAnterior,
                    M.Saldo,
                    M.UserId,
                    M.Estorno,
                    M.Perca,
                    M.Transferencia,
                    M.Ignorar,
                    M.Obs
	            FROM 
		            Movimento M
		            INNER JOIN Lote L ON L.Id = M.LoteId
                    INNER JOIN Talhao T ON T.Id = M.TalhaoId
                    INNER JOIN TipoLocal TP ON TP.Id = M.LocalId
                    INNER JOIN TipoProduto P ON P.Id = L.TipoProdutoId 
                    LEFT JOIN User C ON C.Id = M.ClienteId
                ";
        public MovimentoRepository(IConexao conexao) : base(conexao) => _buscarTodosTemplate = _select;

        //Inserir
        public long Inserir(Movimento dominio, IDbTransaction? transaction = null)
        {
            return Insert("Movimento", new
            {
                dominio.Id,
                dominio.EmpresaId,
                dominio.PedidoId,
                dominio.LoteId,
                dominio.Identificador,
                dominio.TalhaoId,
                dominio.LocalId,
                dominio.ClienteId,
                dominio.GramaId,
                dominio.IntervaloDeCorte,
                dominio.Quantidade,
                dominio.Operacao,
                dominio.SaldoAnterior,
                dominio.Saldo,
                dominio.DataCriacao,
                dominio.DataAlteracao,
                dominio.Deletado,
                dominio.UserId,
                dominio.Estorno,
                dominio.Perca,
                dominio.Transferencia,
                dominio.Ignorar,
                dominio.Obs
            }, transaction);
        }

        //Alterar
        public long Alterar(Movimento dominio, IDbTransaction? transaction = null)
        {
            Update("Movimento", new
            {
                dominio.Id,
                dominio.EmpresaId,
                dominio.PedidoId,
                dominio.LoteId,
                dominio.Identificador,
                dominio.TalhaoId,
                dominio.LocalId,
                dominio.ClienteId,
                dominio.GramaId,
                dominio.IntervaloDeCorte,
                dominio.Quantidade,
                dominio.Operacao,
                dominio.SaldoAnterior,
                dominio.Saldo,
                dominio.DataCriacao,
                dominio.DataAlteracao,
                dominio.Deletado,
                dominio.UserId,
                dominio.Estorno,
                dominio.Perca,
                dominio.Transferencia,
                dominio.Ignorar,
                dominio.Obs
            }, transaction);

            return dominio.Id;
        }

        //Salvar
        public long Salvar(Movimento dominio, IDbTransaction? transaction = null)
        {
            var obj = Buscar(dominio, transaction);
            if (obj == null)
                return Inserir(dominio, transaction);
            else
                return Alterar(dominio, transaction);
        }

        //Buscar
        public IEnumerable<Movimento> BuscarTodos(IDbTransaction? transaction = null)
        {
            return _conexao.Query<Movimento>(_select, null, transaction);
        }

        public IEnumerable<Movimento> BuscarTodosFilter(SqlBuilder sqlBuilder, int paginaAtual, out int totalLinhas)
        {
            var sql = sqlBuilder.AddTemplate($"{_select} /**where**/");

            return _conexao.Query<Movimento>(sql.RawSql, paginaAtual, out totalLinhas, sql.Parameters);
        }

        public Movimento Buscar(long Id, IDbTransaction? transaction = null)
        {
            var sql = $"{_select} Where M.Id = @Id AND M.Deletado = 0";
            return _conexao.QueryFirstOrDefault<Movimento>(sql, new { Id }, transaction);
        }

        public Movimento Buscar(Movimento dominio, IDbTransaction? transaction = null)
        {
            return Buscar(dominio.Id, transaction);
        }

        //Excluir
        public void Excluir(long Id, IDbTransaction? transaction = null)
        {
            Delete("Movimento", new { Id }, transaction);
        }

        public void Excluir(Movimento dominio, IDbTransaction? transaction = null)
        {
            Excluir(dominio, transaction);
        }

        //Adicionais
        public Movimento BuscarPorPedido(long pedidoId, IDbTransaction transaction)
        {
            var sql = $"{_select} Where M.PedidoId = @PedidoId AND M.Deletado = 0";
            return _conexao.QueryFirstOrDefault<Movimento>(sql, new { PedidoId = pedidoId }, transaction);
        }

        public Movimento BuscarUltimoMovimento(long LoteId, long TalhaoId, long LocalId, long EmpresaId, IDbTransaction transaction)
        {
            return _conexao.QueryFirstOrDefault<Movimento>
            (
              @$"{_select} Where
                L.Id = @LoteId 
                AND T.Id = @TalhaoId
                AND M.LocalId = @LocalId 
                AND M.EmpresaId = @EmpresaId
                Order By M.Id DESC",
                 new
                 {
                     LoteId,
                     TalhaoId,
                     LocalId,
                     EmpresaId
                 }
                 , transaction
            );
        }
    }
}
