using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Configuration;
using Crud_Facade_Acesso.Dados.Web.Contexto;

namespace Crud_Facade_Acesso.Servicos.Projeto.Web.Contexto
{
    public abstract class ContextoSql : IContextoDAO
    {
        public abstract void Salvar(object entidade);
        public abstract void Alterar(object entidade);
        public abstract void Excluir(object entidade);
        public abstract IList<object> Consultar(object entidade);

        protected SqlConnection conexao;
        protected SqlTransaction transacao;
        protected SqlDataReader dataReader;
        protected SqlCommand comando;

        private ConnectionStringSettings connString;

        public ContextoSql()
        {
            connString = WebConfigurationManager.ConnectionStrings["principal"];
        }

        public bool AbrirConexao()
        {
            if (conexao != null && conexao.State == ConnectionState.Open)
                return false;

            IDbConnection Iconn = new SqlConnection(connString.ConnectionString);
            Iconn.Open();
            conexao = (SqlConnection)Iconn;
            return true;
        }

        public bool FecharConexao()
        {
            if (conexao != null && conexao.State == ConnectionState.Closed)
                return false;
            conexao.Close();
            conexao.Dispose();
            return true;

        }

        public void Commit()
        {
            if (transacao != null)
            {
                transacao.Commit();
                transacao.Dispose();
                transacao = null;

            }
        }

        public void Rollback()
        {
            if (transacao != null)
            {
                transacao.Rollback();
                transacao.Dispose();
                transacao = null;

            }
        }

        public void ComecaTransacao()
        {
            transacao = conexao.BeginTransaction();

        }

        public void CompartilharTransacao(IDbTransaction transacao)
        {
            this.transacao = (SqlTransaction)transacao;
        }

        public System.Data.IDbTransaction RetornaTransacao()
        {
            return transacao;
        }

        public void CompartilhaConexao(System.Data.IDbConnection conexao)
        {
            this.conexao = (SqlConnection)conexao;
        }

        public System.Data.IDbConnection RetornaConexao()
        {
            return conexao;
        }

        protected object TratarNulidade(object valor)
        {
            if (valor == null)
                return DBNull.Value;
            return valor;
        }

        protected object ObjetoOuNulo(int index)
        {
            if (dataReader.IsDBNull(index))
                return null;
            return dataReader.GetValue(index);
        }

    }
}
