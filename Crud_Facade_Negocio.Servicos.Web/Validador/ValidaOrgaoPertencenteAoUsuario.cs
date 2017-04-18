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
    /// Valida se o Usuário trabalha para o órgão informado na Autorização
    /// </summary>
    public class ValidaOrgaoPertencenteAoUsuario : ValidadorAbstrato
    {
        public override string Executar(object entidade)
        {
            Autorizacao auth = entidade as Autorizacao;

            IFachada<Profissional> fachada = new FachadaAdmWeb<Profissional>();
            fachada.SalvaConexaoAtiva(this.conexao); // Manter conexão anterior
            fachada.SalvaTransacaoAtiva(this.transacao); // Manter transação anterior
            fachada.DefineTemQueFecharConexao(false); // Não fechar ao finalizar

            auth.Usuario.OrgaoAtual = auth.OrgaoAutorizado;
            IList<Profissional> retorno = fachada.Consultar(auth.Usuario);

            if (retorno == null)
                return "O Usuário " + auth.Usuario.Codigo + " não trabalha para o órgão " +
                        auth.Usuario.OrgaoAtual.Codigo + " (" + auth.Usuario.OrgaoAtual.Sigla + ")";

            return null;

        }
    }
}

