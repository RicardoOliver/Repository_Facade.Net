using Crud_Facade_Apresentacao_Projeto.Web.Controllers;
using Crud_Facade_Modelos.Web;
using Crud_Facade_Modelos.Web.ViewModel;
using Crud_Facade_Negocio.Base.Web.Interfaces;
using Crud_Facade_Negocio.Servicos.Web.Fachada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Projeto.facade.Net.Controllers
{
    public class AplicativoController : CommonsController
    {
        #region Cadastrar Aplicativo

        /// <summary>
        /// Método responsável por cumprir o requisito de Cadastrar Aplicativo.
        ///     Quando chamado em GET - Retorna a tela de Cadastrar Aplicativo
        ///     Quando chamado em POST - realiza o Cadastro de Aplicativos
        /// </summary>
        /// <returns>A tela de Cadastrar Aplicativo (GET) ou A tela de Index 
        ///          podendo ou não conter uma mensamge de erro (Post)</returns>
        public ActionResult CadastrarAplicativo(AlterarOuSalvarAplicativoView dados)
        {
            #region Decidir se deve proseguir com a requisição
            ActionResult urlRetorno = DecideUrlFormulários("/Aplicativo/CadastrarAplicativo");
            if (urlRetorno != null)
                return urlRetorno; // ação que deve ser retornada para o browser
            #endregion

            Aplicativo aplicativo = new Aplicativo();

            aplicativo = dados.CriarModelo();



            IFachada<Aplicativo> fachada = new FachadaAdmWeb<Aplicativo>();

            string retorno = fachada.Salvar(aplicativo);

            if (retorno != null) //se não retornar null, é porque ocorreu um erro de validação
            {
                ViewBag.Mensagem = retorno;
                dados.ActionDestinos = "/Aplicativo/CadastrarAplicativo";
                return View("CadastrarAplicativo", dados);
            }


            ViewBag.Mensagem = "Cadastrado com sucesso !";
            return View("../Home/Index");
        }

        #endregion

        #region AlterarAplicativo

        public ActionResult AlterarAplicativo(AlterarOuSalvarAplicativoView dados)
        {
            #region Decidir se deve proseguir com a requisição
            ActionResult urlRetorno = DecideUrlFormulários("/Aplicativo/AlterarAplicativo", "ConsultarAplicativo");

            if (urlRetorno != null)
            {
                return urlRetorno; // ação que deve ser retornada para o browser
            }
            #endregion

            Aplicativo aplicativo = new Aplicativo();

            aplicativo = dados.CriarModelo();

            IFachada<Aplicativo> fachada = new FachadaAdmWeb<Aplicativo>();

            string retorno = fachada.Alterar(aplicativo);

            if (retorno != null)//se não retornar null, é porque ocorreu um erro de validação
            {
                ViewBag.Mensagem = retorno;
                AlterarOuSalvarAplicativoView modelo = new AlterarOuSalvarAplicativoView();
                modelo.PreencherDadosView(aplicativo);
                dados.ActionDestinos = "/Aplicativo/AlterarAplicativo";
                return View("CadastrarAplicativo", modelo);
            }

            ViewBag.Mensagem = "Alterado com sucesso!";
            return View("../Home/Index");
        }
        #endregion


        #region ConsultarAplicativo

        public ActionResult ConsultarAplicativo(string CodigoAplicativo)
        {


            Aplicativo aplicativoProcurado = new Aplicativo();
            aplicativoProcurado.Nome = CodigoAplicativo;

            if (aplicativoProcurado == null || aplicativoProcurado.Nome.Trim() == "")
            {
                ViewBag.Mensagem = "Informe o codigo";
                return View("ConsultarAplicativo");
            }

            IFachada<Aplicativo> fachada = new FachadaAdmWeb<Aplicativo>();
            IList<Aplicativo> retorno = fachada.Consultar(aplicativoProcurado);

            if (retorno == null)
            {
                ViewBag.Message = "Aplicativo" + aplicativoProcurado.Nome + "Não Existe";
                return View("ConsultarAplicativo");
            }
            AlterarOuSalvarAplicativoView view = new AlterarOuSalvarAplicativoView();
            view.PreencherDadosView(retorno[0]);
            view.ActionDestinos = "/Aplicativo/AlterarAplicativo";
            return View("CadastrarAplicativo", view);

        }
        #endregion



    }
}

