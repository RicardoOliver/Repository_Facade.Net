using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crud_Facade_Modelo.Web;
using Crud_Facade_Modelo.Web.ViewModel;
using Crud_Facade_Negocios.Base.Web.Interfaces;
using Crud_Facade_Negocios.Servicos.Web.Fachada;

namespace Crud_Facade_Apresentacao_Projeto.Web.Controllers
{
    public class AutorizacaoController : CommonsController
    {
        private IFachada<AutorizarUsuarioViewModel> fachadaVM;

        /// <summary>
        /// Método responsável por cumprir o requisito de Autorizar Usuários Cadastrados em Aplicativos
        /// previamente cadastrados.
        ///     Quando chamado em GET - Retorna a tela de Autorizar Usuário
        ///     Quando chamado em POST - Retorna a tela de Cadastrar
        /// </summary>
        /// <returns>A tela de Cadastrar Aplicativo (GET) ou A tela de Index 
        ///          podendo ou não conter uma mensamge de erro (Post)</returns>
        public ActionResult AutorizarUsuario(AutorizarUsuarioViewModel dados)
        {
            #region Decidir se deve proseguir com a requisição
            AutorizarUsuarioViewModel model = new AutorizarUsuarioViewModel();


            ActionResult urlRetorno = DecideUrlFormulários("/Autorizacao/AutorizarUsuario", model);
            if (urlRetorno != null)
            {
                if (usuario != null) // Usuario preenchido com os dados da seção na superclasse
                {
                    model = PreencheModel(model);
                }
                return urlRetorno; // ação que deve ser retornada para o browser            
            }
            #endregion


            IList<Autorizacao> autorizacoes = new List<Autorizacao>();
            dados.UsuarioLiberacao = usuario;
            autorizacoes = dados.CriarModelo();

            IFachada<Autorizacao> fachada = new FachadaAdmWeb<Autorizacao>();
            string retorno;

            if (Request["Salvar"] != null)
                retorno = fachada.SalvarTodos(autorizacoes);
            else if (Request["Alterar"] != null)
                retorno = fachada.AlterarTodos(autorizacoes);
            else if (Request["Inativar"] != null)
            {
                Autorizacao auxiliar = new Autorizacao();
                auxiliar.OrgaoAutorizado = autorizacoes[0].OrgaoAutorizado;
                auxiliar.Usuario = autorizacoes[0].Usuario;

                retorno = fachada.Excluir(autorizacoes[0]);
            }
            else
                retorno = "Opção Inválida";

            if (retorno != null) // se retornar null, é porque ocorreu tudo bem
            {
                ViewBag.Mensagem = retorno;
                model = PreencheModel(autorizacoes, model);
                return View(model);
            }

            ViewBag.Mensagem = "Autorizado com sucesso!";
            return View("../Home/Index");

        } // AutorizarUsuario

        [HttpPost]
        public ActionResult ConsultarUsuario(AutorizarUsuarioViewModel model)
        {
            #region Decidir se deve proseguir com a requisição    
            Autorizacao auth;
            ActionResult urlRetorno = DecideUrlFormulários("/Autorizacao/AutorizarUsuario", model);
            if (urlRetorno != null)
            {
                if (usuario != null) // Usuario preenchido com os dados da seção na superclasse
                {
                    model = PreencheModel(model);
                }
                return urlRetorno; // ação que deve ser retornada para o browser            
            }

            #endregion

            IFachada<Profissional> fachada = new FachadaAdmWeb<Profissional>();
            fachada.DefineTemQueFecharConexao(false);

            IList<Profissional> resultados = this.ConsultaUsuarioNoBanco(model, fachada);


            if (resultados == null || resultados.Count == 0)
            {
                ViewBag.Mensagem = "Nenhum Usuário encontrado (" + model.NomeUsuario.Trim() + ")";

                // Por padrão, fecha ao finalizar a transação
                fachadaVM = new FachadaAdmWeb<AutorizarUsuarioViewModel>();
                fachadaVM.SalvaConexaoAtiva(fachada.RetornaConexaoAtiva()); // manter conexão
                fachadaVM.SalvaTransacaoAtiva(fachada.RetornaTransacaoAtiva()); // manter trasação

                model = PreencheModel(model);
            }
            else if (resultados.Count == 1)
            {
                auth = new Autorizacao();
                auth.Usuario = resultados[0];
                auth.UsuarioAutorizando = usuario;

                IFachada<Autorizacao> fachadaAuth = new FachadaAdmWeb<Autorizacao>();
                fachadaAuth.SalvaConexaoAtiva(fachada.RetornaConexaoAtiva()); // manter conexao
                fachadaAuth.SalvaTransacaoAtiva(fachada.RetornaTransacaoAtiva()); // manter transacao
                fachadaAuth.DefineTemQueFecharConexao(false); // Não deve fechar

                IList<Autorizacao> autorizacoes = fachadaAuth.Consultar(auth);
                if (autorizacoes == null)
                {
                    autorizacoes = new List<Autorizacao>();
                    if (auth.UsuarioAutorizando.Perfil == Perfil.AUTORIZADOR)
                        auth.OrgaoAutorizado = auth.UsuarioAutorizando.OrgaoAtual;
                    autorizacoes.Add(auth);
                }

                // Por padrão, fecha ao finalizar a transação
                fachadaVM = new FachadaAdmWeb<AutorizarUsuarioViewModel>();
                fachadaVM.SalvaConexaoAtiva(fachadaAuth.RetornaConexaoAtiva()); // manter conexão
                fachadaVM.SalvaTransacaoAtiva(fachadaAuth.RetornaTransacaoAtiva()); // manter trasação


                model = PreencheModel(autorizacoes, model);
            }



            return View("AutorizarUsuario", model);
        }


        [HttpPost]
        public ActionResult CopiarAutorizacao(CopiarAutorizacaoViewModel model, AutorizarUsuarioViewModel modelRetorno)
        {
            #region Decidir se deve proseguir com a requisição

            ActionResult urlRetorno = DecideUrlFormulários("/Autorizacao/AutorizarUsuario", modelRetorno);
            if (urlRetorno != null)
            {
                if (usuario != null) // Usuario preenchido com os dados da seção na superclasse
                {
                    modelRetorno = PreencheModel(modelRetorno);
                }
                return urlRetorno; // ação que deve ser retornada para o browser            
            }
            #endregion

            modelRetorno.UsuarioLiberacao = model.UsuarioLiberacao = usuario;

            IList<Autorizacao> autorizacoes = modelRetorno.CriarModelo();

            CopiaDeAutorizacao copia = model.CriarModelo();

            IList<CopiaDeAutorizacao> retorno = new List<CopiaDeAutorizacao>();
            IFachada<CopiaDeAutorizacao> fachada = new FachadaAdmWeb<CopiaDeAutorizacao>();
            fachada.DefineTemQueFecharConexao(false); // manter transação e conexão aberta

            string msgValidacao = fachada.ConsultarComValidacao(copia, retorno);


            if (msgValidacao != null)
            {
                ViewBag.Mensagem = msgValidacao;

                fachadaVM = new FachadaAdmWeb<AutorizarUsuarioViewModel>();
                fachadaVM.SalvaConexaoAtiva(fachada.RetornaConexaoAtiva()); // mantém conexão anterior
                fachadaVM.SalvaTransacaoAtiva(fachada.RetornaTransacaoAtiva()); // mantém transação anterior


                return View("AutorizarUsuario", PreencheModel(autorizacoes, modelRetorno));
            }
            else if (retorno == null || retorno.Count == 0)
            {
                fachadaVM = new FachadaAdmWeb<AutorizarUsuarioViewModel>(); // Por padrão fecha a conexão
                fachadaVM.SalvaConexaoAtiva(fachada.RetornaConexaoAtiva()); // mantém conexão anterior
                fachadaVM.SalvaTransacaoAtiva(fachada.RetornaTransacaoAtiva()); // mantém transação anterior

                ViewBag.Mensagem = "Não há validações para copiar";


                return View("AutorizarUsuario", PreencheModel(autorizacoes, modelRetorno));
            }



            fachada.DefineTemQueFecharConexao(true); // a próxima operação é a última
            msgValidacao = fachada.SalvarTodos(retorno);

            if (msgValidacao != null)
                ViewBag.Mensagem = msgValidacao;
            else
                ViewBag.Mensagem = "Autorizações copiadas com sucesso.";

            //return View("AutorizarUsuario", PreencheModel(autorizacoes, modelRetorno));
            return View("../Home/Index");
        } // CopiarAutorizações

        #region Métodos privados
        private AutorizarUsuarioViewModel PreencheModel(AutorizarUsuarioViewModel model)
        {
            string copia = model.UsuarioCopia;
            Autorizacao auth = new Autorizacao();
            auth.Usuario = new Profissional();
            auth.Usuario.Codigo = model.NomeUsuario;
            auth.UsuarioAutorizando = usuario;
            auth.OrgaoAutorizado = new Orgao();
            if (auth.UsuarioAutorizando.Perfil != Perfil.GERENTE)
                auth.OrgaoAutorizado.Sigla = usuario.OrgaoAtual.Sigla;

            IList<Autorizacao> autorizacoes = new List<Autorizacao>();
            autorizacoes.Add(auth);

            model.PreencherDadosView(autorizacoes);
            model.UsuarioLiberacao = usuario;
            model = new FachadaAdmWeb<AutorizarUsuarioViewModel>().Consultar(model)[0];

            model.UsuarioCopia = copia;

            return model;
        }

        private AutorizarUsuarioViewModel PreencheModel(IList<Autorizacao> autorizacoes, AutorizarUsuarioViewModel model)
        {
            string copia = model.UsuarioCopia;
            model = new AutorizarUsuarioViewModel();
            model.PreencherDadosView(autorizacoes);
            model.UsuarioCopia = copia;
            model.UsuarioLiberacao = usuario;

            if (fachadaVM == null) // Instanciou a fachadaVM??
                fachadaVM = new FachadaAdmWeb<AutorizarUsuarioViewModel>();

            model = fachadaVM.Consultar(model)[0];

            return model;
        }


        private IList<Profissional> ConsultaUsuarioNoBanco(AutorizarUsuarioViewModel model, IFachada<Profissional> fachada)
        {
            if (model.NomeUsuario == null)
                model.NomeUsuario = "";


            Profissional user = new Profissional { Codigo = model.NomeUsuario.Trim() };
            IList<Profissional> resultados = fachada.Consultar(user);
            fachada = null; // limpando fachada que não será mais utilizada

            return resultados;
        } // ConsultarUsuario

        #endregion
    }
}
