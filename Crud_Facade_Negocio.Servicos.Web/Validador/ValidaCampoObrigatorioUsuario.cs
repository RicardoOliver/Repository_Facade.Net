using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crud_Facade_Modelos.Web;
using Crud_Facade_Negocios.Base.Web.Interfaces;
using Crud_Facade_Negocios.Servicos.Web.Fachada;

namespace Crud_Facade_Negocios.Servicos.Web.Validador
{/// <summary>
 /// Valida se os campos obrigatórios do usuário foram informados no momento do cadastro
 /// </summary>
    public class ValidaCampoObrigatorioUsuario : ValidadorAbstrato
    {
        public override string Executar(object entidade)
        {
            Profissional usuario = (Profissional)entidade;


            if (usuario.Codigo == null ||
                                  string.IsNullOrEmpty(usuario.Codigo.ToLower()))
            {
                return "O campo código do usuario é obrigatório *";
            }

            if (usuario.Nome == null ||
                           string.IsNullOrEmpty(usuario.Nome.ToUpper()))
            {
                return "O campo nome do usuario é obrigatório *";
            }


            if (usuario.OrgaoPadrao == null || usuario.OrgaoPadrao.Codigo == null ||
                string.IsNullOrEmpty(usuario.OrgaoPadrao.Codigo.ToLower()) &&
               (usuario.OrgaoPadrao.Sigla == null ||
               string.IsNullOrEmpty(usuario.OrgaoPadrao.Sigla.Trim())))
            {
                return " O campo código ou sigla  do órgão padrão  do usuario é obrigatório *";
            }


            for (int i = 0; i < usuario.OrgaosDoUsuario.Count; i++)
            {


                if (usuario.OrgaosDoUsuario[i].Codigo == null ||
                   string.IsNullOrEmpty(usuario.OrgaosDoUsuario[i].Codigo.ToLower()) &&
                   (usuario.OrgaosDoUsuario[i].Sigla == null ||
                   string.IsNullOrEmpty(usuario.OrgaosDoUsuario[i].Sigla.Trim())))
                {
                    return "O campo código ou sigla  do orgão que o usuário trabalha  é obrigatório *";
                }

            }

            return null;

        }
    }
}


