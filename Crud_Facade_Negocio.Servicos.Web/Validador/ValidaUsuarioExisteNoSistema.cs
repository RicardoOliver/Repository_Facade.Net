using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crud_Facade_Modelos.Web;
using Crud_Facade_Negocios.Base.Web.Interfaces;
using Crud_Facade_Negocios.Servicos.Web.Fachada;
using Crud_Facade_Negocio.Servicos.Web.Fachada;

namespace Crud_Facade_Negocios.Servicos.Web.Validador
{
    /// <summary>
    /// Valida APENAS se o usuário existe na base.
    /// </summary>
    public class ValidaUsuarioExisteNoSistema : ValidadorAbstrato
    {
        public override string Executar(object entidade)
        {
            if (entidade is CopiaDeAutorizacao)
            {
                CopiaDeAutorizacao copia = entidade as CopiaDeAutorizacao;
                string msg = this.Executar(copia.AutorizacaoOrigem);
                if (msg == null)
                    msg = this.Executar(copia.AutorizacaoDestino);
                return msg;
            }
            Autorizacao auth = entidade as Autorizacao;
            IFachada<Profissional> fachada = new FachadaAdmWeb<Profissional>();
            fachada.SalvaConexaoAtiva(this.conexao); // Manter conexão anterior
            fachada.SalvaTransacaoAtiva(this.transacao); // Manter transação anterior
            fachada.DefineTemQueFecharConexao(false); // Não fechar ao finalizar

            Orgao aux = auth.Usuario.OrgaoAtual;
            auth.Usuario.OrgaoAtual = null;
            IList<Profissional> retorno = fachada.Consultar(auth.Usuario);

            auth.Usuario.OrgaoAtual = aux;

            if (retorno == null)
                return "O Usuário informado não existe!";

            return null;
        }
    }
}


