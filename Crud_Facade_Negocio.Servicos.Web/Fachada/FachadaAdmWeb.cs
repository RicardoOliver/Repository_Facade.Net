using Crud_Facade_Acesso.Servicos.Projeto.Web.Contexto;
using Crud_Facade_Modelos.Web;
using Crud_Facade_Modelos.Web.ViewModel;
using Crud_Facade_Negocios.Base.Web.Interfaces;
using Crud_Facade_Negocios.Servicos.Web.Fachada;
using Crud_Facade_Negocios.Servicos.Web.Validador;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;




namespace Crud_Facade_Negocio.Servicos.Web.Fachada
{
    public class FachadaAdmWeb<Tipo> : FachadaGenerica<Tipo>
    {
        private void Iniciar()
        {//
            IList<IValidador> validadores = new List<IValidador>();

            #region Autorização
            if (typeof(Tipo) == typeof(Autorizacao))
            {
                IValidador somenteSalvar = new ValidaAutorizacaoRepetida();
                validadores = null;
                validadores = new List<IValidador>();
                validadores.Add(new ValidaCamposAutorizacao());
                validadores.Add(new ValidaRotinasInformadas());
                validadores.Add(new ValidaUsuarioExisteNoSistema());
                validadores.Add(new ValidaOrgaoExistente());
                validadores.Add(new ValidaOrgaoPertencenteAoUsuario());
                validadores.Add(somenteSalvar); // ValidaAutorizacaoRepetida
                validadores.Add(new ValidaPermissoesAutorizacao());
                this.ValidadoresSalvar.Add(typeof(Autorizacao).Name, validadores);
                this.ValidadoresSalvarTodos.Add(typeof(Autorizacao).Name, validadores);

                IList<IValidador> auxiliar = new List<IValidador>();
                auxiliar = auxiliar.Concat(validadores).ToList<IValidador>();
                auxiliar.Remove(somenteSalvar); // Só valida AutorizacaoRepetida no Salvar                                                
                this.ValidadoresAlterarTodos.Add(typeof(Autorizacao).Name, auxiliar);

                validadores = new List<IValidador>();
                validadores.Add(new ValidaCamposInativaAutorizacoes());
                validadores.Add(new ValidaUsuarioExisteNoSistema());
                validadores.Add(new ValidaOrgaoExistente());
                validadores.Add(new ValidaOrgaoPertencenteAoUsuario());
                this.ValidadoresExcluirTodos.Add(typeof(Autorizacao).Name, validadores);
                this.ValidadoresExcluir.Add(typeof(Autorizacao).Name, validadores);

                this.Daos.Add(typeof(Autorizacao).Name, new ContextoAutorizacaoViewModel());

                return;
            }
            #endregion

            #region Aplicativo
            if (typeof(Tipo) == typeof(Aplicativo))
            {
                validadores = null;
                validadores = new List<IValidador>();
                validadores.Add(new ValidaCampoObrigatorioAplicativo());
                validadores.Add(new ValidaMenusDiferentes());
                validadores.Add(new ValidaAplicativoExistente());
                this.ValidadoresSalvar.Add(typeof(Aplicativo).Name, validadores);

                validadores = new List<IValidador>();
                validadores.Add(new ValidaCampoObrigatorioAplicativo());
                this.ValidadoresAlterar.Add(typeof(Aplicativo).Name, validadores);
                this.ValidadoresExcluir.Add(typeof(Aplicativo).Name, validadores);

                this.Daos.Add(typeof(Aplicativo).Name, new ContextoAplicativo());

                return;
            }
            #endregion

            #region  Usuario
            if (typeof(Tipo) == typeof(Profissional))
            {
                validadores = null;
                validadores = new List<IValidador>();
                validadores.Add(new ValidaCampoObrigatorioUsuario());
                validadores.Add(new ValidaUsuarioExistente());
                validadores.Add(new ValidarOrgaosDoUsuario());
                this.ValidadoresSalvar.Add(typeof(Profissional).Name, validadores);

                validadores = new List<IValidador>();
                validadores.Add(new ValidaCampoObrigatorioUsuario());
                validadores.Add(new ValidarOrgaosDoUsuario());

                this.ValidadoresAlterar.Add(typeof(Profissional).Name, validadores);
                this.ValidadoresExcluir.Add(typeof(Profissional).Name, validadores);

                this.Daos.Add(typeof(Profissional).Name, new ContextoUsuario());

                return;
            }
            #endregion

            #region Orgão
            if (typeof(Orgao) == typeof(Tipo))
            {
                this.Daos.Add(typeof(Orgao).Name, new ContextoOrgao());
                return;
            }
            #endregion

            #region AutorizarUsuarioViewModel
            if (typeof(Tipo) == typeof(AutorizarUsuarioViewModel))
            {
                this.Daos.Add(typeof(AutorizarUsuarioViewModel).Name, new ContextoAutorizacaoViewModel());
                return;
            }
            #endregion

            #region CopiaDeAutorizacao
            if (typeof(Tipo) == typeof(CopiaDeAutorizacao))
            {
                validadores.Add(new ValidaCamposCopiaDeAutorizacao());
                validadores.Add(new ValidaUsuarioExisteNoSistema());

                ValidadoresConsultar.Add(typeof(CopiaDeAutorizacao).Name, validadores);


                Daos.Add(typeof(CopiaDeAutorizacao).Name, new ContextoCopiaDeAutorizacao());
                return;
            }
            #endregion

            #region Liberacao
            if (typeof(Tipo) == typeof(SubMenu))
            {
                this.Daos.Add(typeof(SubMenu).Name, new ContextoSubMenusAutorizados());
                return;
            }
            #endregion

        } // FachadaAdmWeb

        public FachadaAdmWeb()
            : base()
        {
            this.Iniciar();
        }



    } // Definição da classe
}
