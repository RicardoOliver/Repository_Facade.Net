using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Modelos.Web.ViewModel
{
    public class AutorizarUsuarioViewModel : IViewModel<IList<Autorizacao>>
    {
        public string UsuarioCopia { get; set; }

        public string NomeUsuario { get; set; }

        public string NomeOrgao { get; set; }

        public IList<string> OrgaosRetorno { get; set; }

        public IList<string> DescricaoOrgaos { get; set; }

        public IList<string> NomeRotinas { get; set; }

        public IList<string> PermissoesIncluir { get; set; }

        public IList<string> PermissoesExcluir { get; set; }

        public IList<string> PermissoesAlterar { get; set; }

        public bool PermiteInformarOrgao { get; private set; }

        public Profissional UsuarioLiberacao { get; set; }

        public IList<string> RotinasDisponíveis { get; set; }

        public void PreencherDadosView(IList<Autorizacao> autorizacoes)
        {
            bool verificouPermitirInformarOrgao = false;
            PermiteInformarOrgao = false;
            NomeRotinas = new List<string>();
            DescricaoOrgaos = new List<string>();
            OrgaosRetorno = new List<string>();
            PermissoesIncluir = new List<string>();
            PermissoesAlterar = new List<string>();
            PermissoesExcluir = new List<string>();

            foreach (Autorizacao auth in autorizacoes)
            {
                if (auth.Usuario != null && !verificouPermitirInformarOrgao)
                {
                    NomeUsuario = auth.Usuario.Codigo;

                    if (auth.UsuarioAutorizando.Perfil == Perfil.GERENTE)
                        PermiteInformarOrgao = true;
                    verificouPermitirInformarOrgao = true;
                }

                if (auth.OrgaoAutorizado != null)
                {
                    OrgaosRetorno.Add(auth.OrgaoAutorizado.Sigla);
                    NomeOrgao = auth.OrgaoAutorizado.Sigla;
                    DescricaoOrgaos.Add(auth.OrgaoAutorizado.Descricao);
                }

                if (auth.Aplicativo != null)
                {


                    if (auth.Aplicativo.Menus == null)
                        auth.Aplicativo.Menus = new List<Menu>();

                    foreach (Menu m in auth.Aplicativo.Menus)
                    {

                        if (m.SubMenus == null)
                            continue;

                        foreach (SubMenu s in m.SubMenus)
                        {

                            NomeRotinas.Add(s.Descricao);


                            if (s.Liberacao != null)
                            {
                                UsuarioLiberacao = s.Liberacao.UsuarioLiberacao;

                                if (s.Liberacao.Permissoes != null)
                                {
                                    PermissoesIncluir.Add(s.Liberacao.Permissoes.
                                        PermissaoIncluir == true ? "S" : "N");
                                    PermissoesExcluir.Add(s.Liberacao.Permissoes.
                                        PermissaoExcluir == true ? "S" : "N");
                                    PermissoesAlterar.Add(s.Liberacao.Permissoes.
                                        PermissaoAlterar == true ? "S" : "N");
                                } // if

                            } // if

                        } // foreach SubMenu

                    } // foreach Menu

                } // if (auth.Aplicativos != null)

            } // foreach Autorizacoes

            if (NomeRotinas.Count == 0)
            {
                NomeRotinas.Add("");
                DescricaoOrgaos.Add("");
                NomeOrgao = "";
                OrgaosRetorno.Add("");
                PermissoesIncluir.Add("");
                PermissoesAlterar.Add("");
                PermissoesExcluir.Add("");
            }


        } // Preencher Dados view

        public IList<Autorizacao> CriarModelo()
        {
            IList<Autorizacao> retorno = null;

            if (NomeRotinas != null && NomeRotinas.Count > 0)
                retorno = new List<Autorizacao>();



            for (int i = 0; i < NomeRotinas.Count; i++)
            {

                Autorizacao auth = new Autorizacao();


                auth.Aplicativo = new Aplicativo { Nome = "", Menus = new List<Menu>() };
                auth.Aplicativo.Menus.Add(new Menu { Nome = "", SubMenus = new List<SubMenu>() });
                auth.Aplicativo.Menus[0].SubMenus.Add(new SubMenu
                {
                    Descricao = NomeRotinas[i],
                });

                Permissoes p = new Permissoes();

                p.PermissaoIncluir = PermissoesIncluir[i] == "S";
                p.PermissaoExcluir = PermissoesExcluir[i] == "S";
                p.PermissaoAlterar = PermissoesAlterar[i] == "S";
                auth.Aplicativo.Menus[0].SubMenus[0].Liberacao =
                    new Liberacao
                    {
                        UsuarioLiberacao = this.UsuarioLiberacao,
                        Permissoes = p,
                    };

                string sigla, codigo;
                if (UsuarioLiberacao.Perfil == Perfil.AUTORIZADOR)
                {
                    sigla = UsuarioLiberacao.OrgaoAtual.Sigla;
                    codigo = UsuarioLiberacao.OrgaoAtual.Codigo;
                }
                else
                {
                    sigla = this.NomeOrgao;
                    codigo = this.NomeOrgao;
                }


                if (this.UsuarioLiberacao.Perfil != Perfil.GERENTE)
                {
                    auth.OrgaoAutorizado.Codigo = UsuarioLiberacao.OrgaoAtual.Codigo;
                    auth.OrgaoAutorizado.Sigla = UsuarioLiberacao.OrgaoAtual.Sigla;
                }
                else
                    auth.OrgaoAutorizado = new Orgao { Codigo = codigo, Sigla = sigla };


                auth.UsuarioAutorizando = UsuarioLiberacao;
                auth.Usuario = new Profissional { Codigo = NomeUsuario };
                retorno.Add(auth);
            } // for

            return retorno;
        }
    }
}


