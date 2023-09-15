using Dapper;
using MultipleConnect.Entidades;
using System.Data;

namespace MultipleConnect.Repositorios
{
    public class PedidoRepository : Repository, IRepositorio<Pedido>
    {
        private string _select =
            @"SELECT 
	                P.Id,
                    P.EmpresaId,
                    P.ClienteId,
                    P.LoteId,
                    P.LocalId,
                    TL.Descricao LocalNome,
                    L.TalhaoId,
                    T.Nome TalhaoNome,
                    P.Quantidade,
                    P.ProdutoId,
                    TP.Descricao ProdutoNome,
                    P.GramaId,   
                    TG.Descricao GramaNome,
                    P.DataCriacao,
                    P.DataAlteracao,
                    P.Deletado,
                    P.Status,
                    L.Identificador LoteIdentificador,
                    C.Nome ClienteNome,
                    P.PrecoPedido,
                    P.PrecoFrete,
                    P.PrecoTotalPedido,
                    P.ResponsavelId,
                    P.FormaPagamentoId,
                    FP.Nome FormaPagamentoNome,
                    R.Nome ResponsavelNome,
                    P.Situacao
                FROM 
	                Pedido P
                    INNER JOIN TIPOLOCAL TL ON TL.Id = P.LocalId
                    LEFT JOIN LOTE L ON L.Id = P.LoteId
                    LEFT JOIN TALHAO T ON T.id = L.TalhaoId
                    LEFT JOIN USER C ON C.Id = P.ClienteId
                    LEFT JOIN USER R ON R.Id = P.ResponsavelId
                    LEFT JOIN TipoProduto TP ON TP.Id = P.ProdutoId
                    LEFT JOIN TipoGrama TG ON TG.Id = P.GramaId
                    LEFT JOIN FormaPagamento FP ON FP.Id = P.FormaPagamentoId
                ";
        public PedidoRepository(IConexao conexao) : base(conexao) => _buscarTodosTemplate = _select;

        //Inserir
        public long Inserir(Pedido dominio, IDbTransaction? transaction = null)
        {
            return Insert("Pedido", new
            {
                dominio.Id,
                dominio.EmpresaId,
                dominio.ClienteId,
                dominio.LoteId,
                dominio.LocalId,
                dominio.Quantidade,
                dominio.ProdutoId,
                dominio.GramaId,
                dominio.DataCriacao,
                dominio.DataAlteracao,
                dominio.Deletado,
                dominio.Status,
                dominio.PrecoPedido,
                dominio.PrecoFrete,
                dominio.PrecoTotalPedido,
                dominio.ResponsavelId,
                dominio.Situacao,
                dominio.FormaPagamentoId
            }, transaction);
        }

        //Alterar
        public long Alterar(Pedido dominio, IDbTransaction? transaction = null)
        {
            Update("Pedido", new
            {
                dominio.Id,
                dominio.EmpresaId,
                dominio.ClienteId,
                dominio.LoteId,
                dominio.LocalId,
                dominio.Quantidade,
                dominio.ProdutoId,
                dominio.GramaId,
                dominio.DataCriacao,
                dominio.DataAlteracao,
                dominio.Deletado,
                dominio.Status,
                dominio.PrecoPedido,
                dominio.PrecoFrete,
                dominio.PrecoTotalPedido,
                dominio.ResponsavelId,
                dominio.Situacao,
                dominio.FormaPagamentoId
            }, transaction);

            return dominio.Id;
        }

        //Salvar
        public long Salvar(Pedido dominio, IDbTransaction? transaction = null)
        {
            var obj = Buscar(dominio, transaction);
            if (obj == null)
                return Inserir(dominio, transaction);
            else
                return Alterar(dominio, transaction);
        }

        //Buscar
        public IEnumerable<Pedido> BuscarTodos(IDbTransaction? transaction = null)
        {
            return _conexao.Query<Pedido>(_select, null, transaction);
        }

        public IEnumerable<Pedido> BuscarTodosFilter(SqlBuilder sqlBuilder, int paginaAtual, out int totalLinhas)
        {
            var sql = sqlBuilder.AddTemplate($"{_select} /**where**/");

            return _conexao.Query<Pedido>(sql.RawSql, paginaAtual, out totalLinhas, sql.Parameters);
        }

        public Pedido Buscar(long Id, IDbTransaction? transaction = null)
        {
            var sqlBuscar = $"{_select} Where P.Id = @Id AND P.Deletado = 0";
            return _conexao.QueryFirstOrDefault<Pedido>(sqlBuscar, new { Id }, transaction);
        }

        public Pedido Buscar(Pedido dominio, IDbTransaction? transaction = null)
        {
            return Buscar(dominio.Id, transaction);
        }

        //Excluir
        public void Excluir(long Id, IDbTransaction? transaction = null)
        {
            Delete("Pedido", new { Id }, transaction);
        }

        public void Excluir(Pedido dominio, IDbTransaction? transaction = null)
        {
            Excluir(dominio, transaction);
        }

        //Adicionais
    }
}
