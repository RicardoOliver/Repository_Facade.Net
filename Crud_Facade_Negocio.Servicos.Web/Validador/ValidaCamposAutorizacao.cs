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
    /// Valida os campos obrigatórios de uma autorização
    /// </summary>
    public class ValidaCamposAutorizacao : ValidadorAbstrato
    {
        public override string Executar(object entidade)
        {
            Autorizacao autorizacao = (Autorizacao)entidade;

            if (autorizacao.Usuario == null || autorizacao.Usuario.Codigo == null)
                return "O Usuário é um campo obrigatório";

            if (autorizacao.OrgaoAutorizado == null ||
                string.IsNullOrEmpty(autorizacao.OrgaoAutorizado.Codigo) ||
                string.IsNullOrEmpty(autorizacao.OrgaoAutorizado.Sigla))
                return "O Órgão é um campo obrigatório";

            if (autorizacao.Aplicativo == null)
                return "As aplicações são obrigatórias";

            Aplicativo app = autorizacao.Aplicativo;

            if (app.Menus == null || app.Menus.Count == 0)
                return "Os menus são obrigatório";

            foreach (Menu m in app.Menus)
            {
                if (m.SubMenus == null ||
                    m.SubMenus.Count == 0)
                    return "A rotina autorizada é obrigatória!";

                foreach (SubMenu s in m.SubMenus)
                {
                    if (string.IsNullOrEmpty(s.Descricao))
                        return "A rotina autorizada é obrigatória";

                    if (s.Liberacao == null)
                        return "Erro de formulário!";

                    if (s.Liberacao.UsuarioLiberacao == null ||
                        string.IsNullOrEmpty(s.Liberacao.UsuarioLiberacao.Codigo))
                        return "Liberação Inválida";

                    if (s.Liberacao.Permissoes == null)
                        return "Erro de formulário";

                } // foreach SubMenu
            } // foreach Menu



            return null;

        }
    }
}

