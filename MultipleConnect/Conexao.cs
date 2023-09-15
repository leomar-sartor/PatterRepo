using Dapper;
using MySqlConnector;
using System.Data;

namespace MultipleConnect
{
    public class Conexao : IConexao, IDisposable
    {
        #region Variaveis
        public string _stringConnection = "server=mysql.meuagro.net;database=meuagro;uid=meuagro;pwd=mr889030;port=3306;Allow User Variables=True;IgnoreCommandTransaction=true";

        public readonly MySqlConnection _connection;
        private ConnectionState _state;

        public string _preSQL { get; set; } = "set session sql_mode='TRADITIONAL';";
        #endregion

        //Construtor
        public Conexao()
        {
            _connection = new MySqlConnection(_stringConnection);
        }

        //Transaction
        public IDbTransaction BeginTransaction()
        {
            CheckState();

            return _connection.BeginTransaction();
        }

        public IDbTransaction BeginTransactionWithLevel(IsolationLevel? level = null)
        {
            CheckState();

            if (level != null)
                return _connection.BeginTransaction((IsolationLevel)level);

            return _connection.BeginTransaction();
        }

        //Primeiro

        public T QueryFirst<T>(string sql, object? param = null, IDbTransaction? transacao = null)
        {
            try
            {
                sql = _preSQL + sql.ToLower();

                CheckState();

                return _connection.QueryFirst<T>(sql.ToLower(), param, transacao);
            }
            catch (Exception ex)
            {
                throw new Exception(sql, ex);
            }
        }
        public T QueryFirstOrDefault<T>(string sql, object? param = null, IDbTransaction? transacao = null)
        {
            try
            {
                sql = _preSQL + sql.ToLower();
                
                CheckState();

                return _connection.QueryFirstOrDefault<T>(sql.ToLower(), param, transacao);
            }
            catch (Exception ex)
            {
                throw new Exception(sql, ex);
            }
        }

        //Busca
        public IEnumerable<T> Query<T>(string sql, object? param = null, IDbTransaction? transacao = null)
        {
            try
            {
                CheckState();

                sql = _preSQL + sql.ToLower();
                return _connection.Query<T>(sql.ToLower(), param, transacao);
            }
            catch (Exception ex)
            {
                throw new Exception(sql, ex);
            }
        }

        //Paginacao
        public IEnumerable<T> Query<T>(string sql, int pagina, out int totalLinhas, object? param = null, short porPagina = 10)
        {
            var offset = 0;
            if (pagina > 1)
                offset = (porPagina * (pagina - 1));

            var totalSQL = _preSQL;
            totalSQL += $"select count(*) from ({sql}) as X";
            totalLinhas = QueryFirst<int>(totalSQL, param );

            sql = $"{_preSQL}{sql} limit {porPagina} offset {offset}";
            return Query<T>(sql, param);
        }

        //Execute
        public void Execute(string sql, object? param = null, IDbTransaction? transacao = null)
        {
            try
            {
                sql = _preSQL + sql.ToLower();

                CheckState();

                _connection.Execute(sql.ToLower(), param, transacao);
            }
            catch (Exception e)
            {
                throw new Exception(sql, e);
            }
        }
        public long Execute<T>(string sql, object? param = null, IDbTransaction? transacao = null)
        {
            try
            {
                var pontoVirgula = sql[sql.Length - 1] == ';' ? "" : ";";
                sql = _preSQL + sql;
                sql = $"{sql}{pontoVirgula}SELECT LAST_INSERT_ID();";
                var Id = _connection.Query<long>(sql.ToLower(), param, transacao).Single();
                return Id;
            }
            catch (Exception e)
            {
                throw new Exception(sql, e);
            }
        }

        //Estado Conexão
        public void CheckState()
        {
            _state = _connection.State;

            try
            {
                if (_state == ConnectionState.Closed)
                {
                    _connection.Open();
                    return;
                }

                if (_state == ConnectionState.Connecting)
                {
                    while (_state == ConnectionState.Connecting)
                    {
                        _state = _connection.State;

                        if (_state != ConnectionState.Connecting)
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Deu erro de Conexão com o Banco de Dados!", ex);
            }

        }

        public void Dispose()
        {
            //_connection?.Close();
            _connection?.Dispose();
        }
    }
}
