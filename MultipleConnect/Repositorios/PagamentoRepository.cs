using Dapper;
using MultipleConnect.Entidades;
using System.Data;

namespace MultipleConnect.Repositorios
{
    public class PagamentoRepository : Repository, IRepositorio<Pagamento>
    {
        private string _select =
            @"SELECT 
	                P.Id,
                    P.EmpresaId,
                    P.UserId,
                    P.Valor,
                    P.Acrescimo,
                    P.Desconto,
                    P.Obs,
                    P.DataCriacao,
                    P.DataAlteracao,
                    P.Deletado,
                    P.DuplicataId,
                    P.Status
                FROM 
	                Pagamento P
                ";
        public PagamentoRepository(IConexao conexao) : base(conexao) => _buscarTodosTemplate = _select;

        //Inserir
        public long Inserir(Pagamento dominio, IDbTransaction? transaction = null)
        {
            return Insert("Pagamento", new
            {
                dominio.Id,
                dominio.UserId,
                dominio.EmpresaId,
                dominio.Valor,
                dominio.Acrescimo,
                dominio.Desconto,
                Obs = dominio.Observacao,
                dominio.DataCriacao,
                dominio.DataAlteracao,
                dominio.Deletado,
                dominio.DuplicataId,
                dominio.Status
            }, transaction);
        }

        //Alterar
        public long Alterar(Pagamento dominio, IDbTransaction? transaction = null)
        {
            Update("Pagamento", new
            {
                dominio.Id,
                dominio.UserId,
                dominio.EmpresaId,
                dominio.Valor,
                dominio.Acrescimo,
                dominio.Desconto,
                Obs = dominio.Observacao,
                dominio.DataCriacao,
                dominio.DataAlteracao,
                dominio.Deletado,
                dominio.DuplicataId,
                dominio.Status
            }, transaction);

            return dominio.Id;
        }

        //Salvar
        public long Salvar(Pagamento dominio, IDbTransaction? transaction = null)
        {
            var obj = Buscar(dominio, transaction);
            if (obj == null)
                return Inserir(dominio, transaction);
            else
                return Alterar(dominio, transaction);
        }

        //Buscar
        public IEnumerable<Pagamento> BuscarTodos(IDbTransaction? transaction = null)
        {
            return _conexao.Query<Pagamento>(_select, null, transaction);
        }

        public IEnumerable<Pagamento> BuscarTodosFilter(SqlBuilder sqlBuilder, int paginaAtual, out int totalLinhas)
        {
            var sql = sqlBuilder.AddTemplate($"{_select} /**where**/");

            return _conexao.Query<Pagamento>(sql.RawSql, paginaAtual, out totalLinhas, sql.Parameters);
        }

        public Pagamento Buscar(long Id, IDbTransaction? transaction = null)
        {
            var sqlBuscar = $"{_select} Where P.Id = @Id AND P.Deletado = 0";
            return _conexao.QueryFirstOrDefault<Pagamento>(sqlBuscar, new { Id }, transaction);
        }

        public Pagamento Buscar(Pagamento dominio, IDbTransaction? transaction = null)
        {
            return Buscar(dominio.Id, transaction);
        }

        //Excluir
        public void Excluir(long Id, IDbTransaction? transaction = null)
        {
            Delete("Pagamento", new { Id }, transaction);
        }

        public void Excluir(Pagamento dominio, IDbTransaction? transaction = null)
        {
            Excluir(dominio, transaction);
        }

        //Adicionais
        public IEnumerable<Pagamento> BuscarPorDuplicata(long DuplicateId, IDbTransaction? transaction = null)
        {
            var sql = $"{_select} Where P.DuplicataId = @DuplicataId AND P.Deletado = 0";
            return _conexao.Query<Pagamento>(sql, new { DuplicataId = DuplicateId }, transaction);
        }

    }
}
