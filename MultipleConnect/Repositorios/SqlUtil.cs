using System.Data;

namespace MultipleConnect.Repositorios
{
    public class SqlUtil
    {
        public IConexao _conexao;

        public SqlUtil(IConexao conexao)
        {
            _conexao = conexao;
        }

        public long Insert(string nomeTabela, object colunas, IDbTransaction? transacao = null)
        {
            var setColunas = new List<string>();
            var setValores = new List<string>();
            var objColunas = colunas.GetType().GetProperties();

            foreach (var coluna in objColunas)
            {
                setColunas.Add(coluna.Name);
                setValores.Add($"@{coluna.Name}");
            }

            var sql = $"INSERT INTO {nomeTabela} ({string.Join(",", setColunas)}) VALUES ({string.Join(",", setValores)})";

            return _conexao.Execute<long>(sql, colunas, transacao);
        }

        public void Update(string nomeTabela, object colunas, IDbTransaction? transacao = null, string chave = "Id" )
        {
            var setColunas = new List<string>();
            var objColunas = colunas.GetType().GetProperties();

            foreach (var coluna in objColunas)
            {
                var nome = coluna.Name;
                if (nome == chave)
                    continue;

                setColunas.Add($"{nome} = @{nome}");
            }

            var sql = $"UPDATE {nomeTabela} SET {string.Join(",", setColunas)} WHERE {chave} = @{chave}";

            _conexao.Execute(sql, colunas, transacao);
        }

        public void Delete(string nomeTabela, object colunas, IDbTransaction? transacao = null, string chave = "Id")
        {
            var sql = $"DELETE FROM {nomeTabela} WHERE {chave} = @{chave}";
            _conexao.Execute(sql, colunas, transacao);
        }

    }
}
