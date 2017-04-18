using Crud_Facade_Modelo.Web;
using Crud_Facade_Modelo.Web.ViewModel;
using Crud_Facade_Negocios.Base.Web.Interfaces;
using Crud_Facade_Negocios.Servicos.Web.Fachada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crud_Facade_Apresentacao_Projeto.Web.Controllers
{
    public class ProfissionalController : CommonsController
    {
        /// <summary>
        /// Método responsável por cumprir o requisito de Cadastrar Usuários
        ///     Quando chamado em GET - Retorna a tela de Cadastrar Usuário
        ///     Quando chamado em POST - Retorna o Cadastro de Usuário
        /// </summary>
        /// <returns>A tela de Cadastrar Aplicativo (GET) ou A tela de Index 
        ///          podendo ou não conter uma mensamge de erro (Post)</returns>

        #region CadastrarUsuario
        public ActionResult CadastrarUsuario(AlterarOuSalvarUsuarioView dados)
        {
            #region Decidir se deve proseguir com a requisição
            ActionResult urlRetorno = DecideUrlFormulários("/Profissional/CadastrarUsuario");
            if (urlRetorno != null)
                return urlRetorno; // ação que deve ser retornada para o browser
            #endregion

            Profissional usuario = new Profissional();

            usuario = dados.CriarModelo();



            IFachada<Profissional> fachada = new FachadaAdmWeb<Profissional>();

            string retorno = fachada.Salvar(usuario);

            if (retorno != null) //se não retornar null, é porque ocorreu um erro de validação
            {
                ViewBag.Mensagem = retorno;
                dados.ActionDestino = "/Profissional/CadastrarUsuario";
                return View("CadastrarUsuario", dados);

            }

            ViewBag.Mensagem = "Cadastrado com sucesso !";
            return View("../Home/Index");
        }



        #endregion


        #region AlteracaoUsuario

        public ActionResult AlterarUsuario(AlterarOuSalvarUsuarioView dados)
        {
            #region Decidir se deve proseguir com a requisição
            ActionResult urlRetorno = DecideUrlFormulários("/Profissional/AlterarUsuario", "ConsultarUsuario");
            if (urlRetorno != null)
            {
                return urlRetorno; // ação que deve ser retornada para o browser
            }
            #endregion

            Profissional usuario = new Profissional();

            usuario = dados.CriarModelo();



            IFachada<Profissional> fachada = new FachadaAdmWeb<Profissional>();

            string retorno = fachada.Alterar(usuario);

            if (retorno != null)//se não retornar null, é porque ocorreu um erro de validação
            {
                ViewBag.Mensagem = retorno;
                AlterarOuSalvarUsuarioView modelo = new AlterarOuSalvarUsuarioView();
                modelo.PreencherDadosView(usuario);
                dados.ActionDestino = "/Profissional/AlterarUsuario";
                return View("CadastrarUsuario", modelo);

            }

            ViewBag.Mensagem = "Alterado com sucesso!";
            return View("../Home/Index");
        }


        #endregion

        #region ConsultarUsuario

        public ActionResult ConsultarUsuario(string CodigoUsuario)
        {


            Profissional usuarioProcurado = new Profissional();

            usuarioProcurado.Codigo = CodigoUsuario;

            if (usuarioProcurado.Codigo == null || usuarioProcurado.Codigo.Trim() == "")
            {
                ViewBag.Mensagem = "Informe o código";
                return View("ConsultarUsuario");
            }


            IFachada<Profissional> fachada = new FachadaAdmWeb<Profissional>();

            IList<Profissional> retorno = fachada.Consultar(usuarioProcurado);

            if (retorno == null)
            {
                ViewBag.Message = "Usuário " + usuarioProcurado.Codigo + " não existe";
                return View("ConsultarUsuario");
            }

            AlterarOuSalvarUsuarioView view = new AlterarOuSalvarUsuarioView();

            view.PreencherDadosView(retorno[0]);

            view.ActionDestino = "/Profissional/AlterarUsuario";
            return View("CadastrarUsuario", view);

        }

        #endregion


    }
}

