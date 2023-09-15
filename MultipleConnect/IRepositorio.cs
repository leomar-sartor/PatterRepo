using Dapper;
using System.Data;

namespace MultipleConnect
{
    public interface IRepositorio<T>
    {
        long Inserir(T dominio, IDbTransaction? transaction = null);
        long Alterar(T dominio, IDbTransaction? transaction = null);
        long Salvar(T dominio, IDbTransaction? transaction = null);

        T Buscar(long Id, IDbTransaction? transaction = null);
        T Buscar(T dominio, IDbTransaction? transaction = null);
        IEnumerable<T> BuscarTodos(IDbTransaction? transaction = null);
        IEnumerable<T> BuscarTodosFilter(SqlBuilder sqlBuilder, int paginaAtual, out int totalLinhas);

        void Excluir(long Id, IDbTransaction? transaction = null);
        void Excluir(T dominio, IDbTransaction? transaction = null);
    }
}
