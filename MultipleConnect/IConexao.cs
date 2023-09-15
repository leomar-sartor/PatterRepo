using System.Data;

namespace MultipleConnect
{
    public interface IConexao
    {
        //Variaveis
        string _preSQL { get; set; }

        //Transaction
        IDbTransaction BeginTransaction();

        IDbTransaction BeginTransactionWithLevel(IsolationLevel? level = null);

        //Primeiro
        T QueryFirst<T>(string sql, object? param = null, IDbTransaction? transacao = null);
        T QueryFirstOrDefault<T>(string sql, object? param = null, IDbTransaction ? transacao = null);
        
        //Busca
        IEnumerable<T> Query<T>(string sql, object? param = null, IDbTransaction ? transacao = null);

        //Paginacao
        IEnumerable<T> Query<T>(string sql, int pagina, out int totalLinhas, object? param = null, short porPagina = 10);

        //Execute
        void Execute(string sql, object? param = null, IDbTransaction? transacao = null);
        long Execute<T>(string sql, object? param = null, IDbTransaction? transacao = null);
    }
}
