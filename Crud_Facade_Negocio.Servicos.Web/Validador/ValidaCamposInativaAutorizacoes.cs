using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crud_Facade_Modelos.Web;
using Crud_Facade_Negocios.Base.Web.Interfaces;
using Crud_Facade_Negocios.Servicos.Web.Fachada;

namespace Crud_Facade_Negocios.Servicos.Web.Validador
{
    public class ValidaCamposInativaAutorizacoes : ValidadorAbstrato
    {
        public override string Executar(object entidade)
        {
            Autorizacao auth = (Autorizacao)entidade;

            // Verificar se preencheu o código do usuário a se desautorizar
            if (auth.Usuario == null || auth.Usuario.Codigo == null ||
                auth.Usuario.Codigo.Trim() == "")
                return "O usuário é um campo obrigatório para fazer a inativação";

            // Verificar se preencheu o orgão a se desautorizar
            if (auth.OrgaoAutorizado == null || auth.OrgaoAutorizado.Codigo == null ||
                auth.OrgaoAutorizado.Codigo.Trim() == "")
                return "Preencha o órgão para fazer a inativação";

            return null; // campos validados

        }
    }
}

