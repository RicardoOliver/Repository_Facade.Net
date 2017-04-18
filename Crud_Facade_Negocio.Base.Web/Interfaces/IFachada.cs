using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crud_Facade_Modelos.Web;

namespace Crud_Facade_Negocios.Base.Web.Interfaces
{
    public interface IFachada<Tipo>
    {
        string Salvar(Tipo entidade);
        string Alterar(Tipo entidade);
        string Excluir(Tipo entidade);
        IList<Tipo> Consultar(Tipo entidade);
        string SalvarTodos(IList<Tipo> entidades);
        string ConsultarComValidacao(Tipo entidade, IList<Tipo> retorno);
        string AlterarTodos(IList<Tipo> entidades);
        string ExcluirTodos(IList<Tipo> entidades);
        void SalvaConexaoAtiva(IDbConnection conexao);
        void SalvaTransacaoAtiva(IDbTransaction transacao);
        IDbConnection RetornaConexaoAtiva();
        IDbTransaction RetornaTransacaoAtiva();
        void DefineTemQueFecharConexao(bool temQueFechar);
        
    }
}

