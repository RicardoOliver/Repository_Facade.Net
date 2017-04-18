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
    /// Valida se os órgãos informados do usuário existem, salvando a sigla e o código do órgão depois
    /// de verificar se o órgão existe
    /// </summary>
    public class ValidarOrgaosDoUsuario : ValidadorAbstrato
    {
        public override string Executar(object entidade)
        {
            Profissional user = entidade as Profissional;

            foreach (Orgao o in user.OrgaosDoUsuario)
            {
                user.OrgaoAtual = o;
                string msg = new ValidaOrgaoExistente().Executar(user);

                if (msg != null)
                    return msg; // retorna mensagem de erro

                o.Codigo = user.OrgaoAtual.Codigo;
                o.Sigla = user.OrgaoAtual.Sigla;

                user.OrgaoAtual = null; // para manter o comportamento correto no contexto do usuário
            }

            return null; // retorna indicando que foi tudo bem

        }
    }
}


