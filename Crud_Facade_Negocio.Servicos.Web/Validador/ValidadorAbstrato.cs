using Crud_Facade_Negocios.Base.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Crud_Facade_Negocios.Servicos.Web.Validador
{
    public abstract class ValidadorAbstrato : IValidador
    {
        protected IDbConnection conexao;
        protected IDbTransaction transacao;

        public abstract string Executar(object entidade);


        public void SalvaConexao(System.Data.IDbConnection conexao)
        {
            this.conexao = conexao;
        }

        public void SalvaTransacao(System.Data.IDbTransaction transacao)
        {
            this.transacao = transacao;
        }
    }
}
