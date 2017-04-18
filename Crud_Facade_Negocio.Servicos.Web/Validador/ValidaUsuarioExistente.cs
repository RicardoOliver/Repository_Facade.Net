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
    /// Valida se o usuário existe na base do .
    /// </summary>
    public class ValidaUsuarioExistente : ValidadorAbstrato
    {
        public override string Executar(object entidade)
        {
            Profissional user = (Profissional)entidade;


            IFachada<Profissional> fachada = new FachadaAdmWeb<Profissional>();
            fachada.SalvaConexaoAtiva(this.conexao); // Manter conexão anterior
            fachada.SalvaTransacaoAtiva(this.transacao); // Manter transação anterior
            fachada.DefineTemQueFecharConexao(false); // Não fechar ao finalizar

            IList<Profissional> codigoUsuarioRetonornado = fachada.Consultar(user);




            if (codigoUsuarioRetonornado != null)
            {
                string retorna = "Usuário já cadastrado.";
                return retorna;
            }



            return null;
        }
    }
}

