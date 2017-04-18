using Crud_Facade_Modelos.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crud_Facade_Apresentacao_Projeto.Web.Controllers
{
    public abstract class CommonsController : Controller
    {
        /// <summary>
        /// Usuario atualmente logado
        /// </summary>
        protected Profissional usuario;

        #region Métodos protegidos



        /// <summary>
        /// Método para verificar se o usuário ainda está logado
        /// </summary>
        /// <returns>false se o usuário estiver logado e a sessão  não tiver expirado. 
        ///          true se não estiver logado (ou a sessão ter expirado)</returns>
        protected bool SessaoExpirou()
        {
            if (Session["Profissional"] != null)
            {
                ViewBag.Profissional = Session["Profissional"] as Profissional;
                usuario = ViewBag.Profissional;
                return false; // a sessão não expirou e o usuário está logado
            }
            return true; // usuário não está logado ou a sessão expirou
        }


        /// <summary>
        /// Método para verificar se uma requisição à algum menu deve seguir adiante ou voltar (seja por
        /// ter expirado a sessão, pelo usuário não ter permissão de acesso ou por estar fazendo uma 
        /// requisição get a determinado formulário (ter clicado em um menu normalmente))
        /// </summary>
        /// <param name="action">a ação sendo executada</param>
        /// <returns>null caso a ação recomendada seja seguir adiante, ou retorna o redirecionamento que
        ///          deve ser feito</returns>
        protected ActionResult DecideUrlFormulários(string action)
        {
            return DecideUrlFormulários(action, null, null);
        }

        /// <summary>
        /// Método para verificar se uma requisição à algum menu deve seguir adiante ou voltar (seja por
        /// ter expirado a sessão, pelo usuário não ter permissão de acesso ou por estar fazendo uma 
        /// requisição get a determinado formulário (ter clicado em um menu normalmente))
        /// </summary>
        /// <param name="p1">a ação sendo executada</param>
        /// <param name="p2">para onde deve retornar caso seja um get</param>
        /// <returns></returns>
        protected ActionResult DecideUrlFormulários(string action, string viewGet)
        {
            return DecideUrlFormulários(action, viewGet, null);
        }

        protected ActionResult DecideUrlFormulários(string action, object model)
        {
            return DecideUrlFormulários(action, null, model);
        }

        protected ActionResult DecideUrlFormulários(string action, string viewGet, object model)
        {
            if (Request.HttpMethod == "GET") // clicou no menu??                            
                return viewGet == null ? View(model) : View(viewGet, model);

            return null; // para continuar a ação do método
        }


    }
}
#endregion