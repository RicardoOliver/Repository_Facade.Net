using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crud_Facade_Modelos.Web;
using Crud_Facade_Negocios.Base.Web.Interfaces;
using Crud_Facade_Negocios.Servicos.Web.Fachada;

namespace Crud_Facade_Negocios.Servicos.Web.Validador
{
    /// <summary>
    /// Verifica se o órgão da autorização (ou do usuário) existem na web
    /// </summary>
    public class ValidaOrgaoExistente : ValidadorAbstrato
    {
        public override string Executar(object entidade)
        {
            Autorizacao auth = entidade as Autorizacao;
            Profissional user = null;

            if (auth == null)
            {
                auth = new Autorizacao();
                user = entidade as Profissional;
                auth.Usuario = user;
                auth.OrgaoAutorizado = user.OrgaoAtual;
            }

            IFachada<Orgao> fachada = new FachadaAdmWeb<Orgao>();
            fachada.SalvaConexaoAtiva(this.conexao); // Manter conexão anterior
            fachada.SalvaTransacaoAtiva(this.transacao); // Manter transação anterior
            fachada.DefineTemQueFecharConexao(false); // Não fechar ao finalizar

            IList<Orgao> retorno;



            retorno = fachada.Consultar(auth.OrgaoAutorizado);

            if (retorno == null)
                return "Orgão informado (" + auth.OrgaoAutorizado.Sigla + ") não existe";

            auth.OrgaoAutorizado = retorno[0];

            if (user != null)
                user.OrgaoAtual = retorno[0];

            return null;




        }
    }
}


