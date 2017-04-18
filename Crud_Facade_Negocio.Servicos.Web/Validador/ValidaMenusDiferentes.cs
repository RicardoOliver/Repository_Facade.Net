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
    /// Valida se não houve Menus repetidos informados em um aplicativo
    /// </summary>
    public class ValidaMenusDiferentes : ValidadorAbstrato
    {
        public override string Executar(object entidade)
        {


            Aplicativo aplicativo = (Aplicativo)entidade;

            IList<string> nomes = new List<string>();
            IList<string> desc = new List<string>();


            for (int i = 0; i < aplicativo.Menus.Count; i++)
            {
                if (nomes.Contains(aplicativo.Menus[i].Nome) == false) // NÃO contém o elemento??
                    nomes.Add(aplicativo.Menus[i].Nome);
                else
                    return "Não deve haver dois menus com o mesmo Nome!";

                if (desc.Contains(aplicativo.Menus[i].Descricao) == false)
                    desc.Add(aplicativo.Menus[i].Descricao);
                else
                    return "Não deve haver dois menus com a mesma descrição!";
            }

            return null;
        }
    }
}

