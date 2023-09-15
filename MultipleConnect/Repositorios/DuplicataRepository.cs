using Dapper;
using MultipleConnect.Entidades;
using System.Data;

namespace MultipleConnect.Repositorios
{
    public class DuplicataRepository : Repository, IRepositorio<Duplicata>
    {
        private string _select =
            @"SELECT 
	                D.Id,
                    D.UserId,
                    D.DataCriacao,
                    D.DataAlteracao,
                    D.Deletado,
                    D.FaturaId,
                    D.NumeroParcela,
                    D.ValorParcela,
                    D.Vencimento,
                    D.Status,
                    D.Desconto,
                    D.Acrescimo
                FROM 
	                Duplicata D
                ";
        public DuplicataRepository(IConexao conexao) : base(conexao) => _buscarTodosTemplate = _select;

        //Inserir
        public long Inserir(Duplicata dominio, IDbTransaction? transaction = null)
        {
            return Insert("Duplicata", new
            {
                dominio.Id,
                dominio.UserId,
                dominio.DataCriacao,
                dominio.DataAlteracao,
                dominio.Deletado,
                dominio.FaturaId,
                dominio.NumeroParcela,
                dominio.ValorParcela,
                dominio.Vencimento,
                dominio.Status,
                dominio.Desconto,
                dominio.Acrescimo
            }, transaction);
        }

        //Alterar
        public long Alterar(Duplicata dominio, IDbTransaction? transaction = null)
        {
            Update("Duplicata", new
            {
                dominio.Id,
                dominio.UserId,
                dominio.DataCriacao,
                dominio.DataAlteracao,
                dominio.Deletado,
                dominio.FaturaId,
                dominio.NumeroParcela,
                dominio.ValorParcela,
                dominio.Vencimento,
                dominio.Status,
                dominio.Desconto,
                dominio.Acrescimo
            }, transaction);

            return dominio.Id;
        }

        //Salvar
        public long Salvar(Duplicata dominio, IDbTransaction? transaction = null)
        {
            var obj = Buscar(dominio, transaction);
            if (obj == null)
                return Inserir(dominio, transaction);
            else
                return Alterar(dominio, transaction);
        }

        //Buscar
        public IEnumerable<Duplicata> BuscarTodos(IDbTransaction? transaction = null)
        {
            return _conexao.Query<Duplicata>(_select, null, transaction);
        }

        public IEnumerable<Duplicata> BuscarTodosFilter(SqlBuilder sqlBuilder, int paginaAtual, out int totalLinhas)
        {
            var sql = sqlBuilder.AddTemplate($"{_select} /**where**/");

            return _conexao.Query<Duplicata>(sql.RawSql, paginaAtual, out totalLinhas, sql.Parameters);
        }

        public Duplicata Buscar(long Id, IDbTransaction? transaction = null)
        {
            var sqlBuscar = $"{_select} Where D.Id = @Id AND D.Deletado = 0";
            return _conexao.QueryFirstOrDefault<Duplicata>(sqlBuscar, new { Id }, transaction);
        }

        public Duplicata Buscar(Duplicata dominio, IDbTransaction? transaction = null)
        {
            return Buscar(dominio.Id, transaction);
        }

        //Excluir
        public void Excluir(long Id, IDbTransaction? transaction = null)
        {
            Delete("Duplicata", new { Id }, transaction);
        }

        public void Excluir(Duplicata dominio, IDbTransaction? transaction = null)
        {
            Excluir(dominio, transaction);
        }

        //Adicionais
        public IEnumerable<Duplicata> BuscarPorFatura(long faturaId, IDbTransaction? transaction = null)
        {
            var sql = $"{_select} Where D.FaturaId = @FaturaId AND D.Deletado = 0";
            return _conexao.Query<Duplicata>(sql, new { FaturaId = faturaId }, transaction);
             
        }
    }
}
