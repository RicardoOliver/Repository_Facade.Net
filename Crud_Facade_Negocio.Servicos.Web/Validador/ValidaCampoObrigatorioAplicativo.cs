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
    /// Valida se os campos obrigatórios do aplicativo foram informados no momento do cadastro
    /// </summary>
    public class ValidaCampoObrigatorioAplicativo : ValidadorAbstrato
    {

        public override string Executar(object entidade)
        {
            Aplicativo aplicativo = (Aplicativo)entidade;

            if (aplicativo.Nome == null || string.IsNullOrEmpty(aplicativo.Nome.Trim()))
                return "O Código de Aplicação é um campo obrigatório * ";

            if (aplicativo.Descricao == null || string.IsNullOrEmpty(aplicativo.Descricao.Trim()))
                return " A Descrição é um campo obrigatório * ";

            if (aplicativo.DescricaoSintetica == null ||
                    string.IsNullOrEmpty(aplicativo.DescricaoSintetica.Trim()))
                return "Descrição Sintética é um campo obrigatório * ";

            if (aplicativo.Versao == null || string.IsNullOrEmpty(aplicativo.Versao.Trim()))
                return " A versao é um campo obrigatório * ";

            if (aplicativo.URLDeExecucao == null || string.IsNullOrEmpty(aplicativo.URLDeExecucao.Trim()))
                return "A URL de Execução é um campo obrigatório * ";

            if (aplicativo.Plataforma == null || string.IsNullOrEmpty(aplicativo.Plataforma.Trim()))
                return "A Plataforma é um campo obrigatório * ";

            if ((aplicativo.URLDeLogoff == null || string.IsNullOrEmpty(aplicativo.URLDeLogoff.Trim()))
                    && aplicativo.Plataforma == "W")
                return "A URL de Logoff é um campo obrigatório * ";

            for (int i = 0; i < aplicativo.Menus.Count; i++)
            {

                if ((aplicativo.Menus[i].Nome == null || string.IsNullOrEmpty(aplicativo.Menus[i].Nome.ToUpper())))
                    return "O nome no menu é um campo obrigatório *";

                if ((aplicativo.Menus[i].Descricao == null ||
                                       string.IsNullOrEmpty(aplicativo.Menus[i].Descricao.Trim())))
                    return " A descricao no menu é um campo obriogatório";

                for (int j = 0; j < aplicativo.Menus[i].SubMenus.Count; j++)
                {
                    if ((aplicativo.Menus[i].SubMenus[j].Nome == null ||
                                           string.IsNullOrEmpty(aplicativo.Menus[i].SubMenus[j].Nome.ToUpper())))
                        return "O nome do subMenu é um campo obrigatório * ";

                    if (aplicativo.Menus[i].SubMenus[j].Descricao == null ||
                                           string.IsNullOrEmpty(aplicativo.Menus[i].SubMenus[j].Descricao.Trim()))
                        return " A descricao do SubMenu é um campo obrigatório * ";

                    if (aplicativo.Menus[i].SubMenus[j].ActionSubMenu == null ||
                                          string.IsNullOrEmpty(aplicativo.Menus[i].SubMenus[j].ActionSubMenu.Trim()))
                        return "O nome do procedimento submenu é um campo obrigatório *";



                }

            }


            return null;


        }
    }
}


