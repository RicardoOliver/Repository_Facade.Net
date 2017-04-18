using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Acesso.Dados.Web.Contexto
{
    public interface IContextoDAO
    {
        void Salvar(object entidade);
        void Alterar(object entidade);
        void Excluir(object entidade);
        IList<object> Consultar(object entidade);
        bool AbrirConexao();
        bool FecharConexao();
        void Commit();
        void Rollback();
        void ComecaTransacao();
        void CompartilharTransacao(IDbTransaction transacao);
        IDbTransaction RetornaTransacao();
        void CompartilhaConexao(IDbConnection conexao);
        IDbConnection RetornaConexao();
    }
}
