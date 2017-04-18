using Crud_Facade_Modelos.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Acesso.Servicos.Projeto.Web.Contexto
{
    public class ContextoAutorizacao : ContextoSql
    {
        private bool JaFoi;

        public ContextoAutorizacao()
        {
            JaFoi = false;
        }

        public override void Salvar(object entidade)
        {
            JaFoi = true; // para não remover autorizações anteriores
            this.Alterar(entidade);
        } // Corpo do método

        public override void Alterar(object entidade)
        {
            Autorizacao autorizacao = (Autorizacao)entidade;
            string sql;

            if (!JaFoi)
            {
                JaFoi = true;
                this.Excluir(entidade);
                /*sql = "Update sis_rel_usuario_submenu set " +
                            "dat_fim_libera = Date('Now') " +
                    "where  cod_usuari = ? and " +
                            "cod_orgao = ? ";
                comando = new IngresCommand(sql, conexao, transacao);
                comando.Parameters.Add(new IngresParameter("cod_usuari", autorizacao.Usuario.Codigo));
                comando.Parameters.Add(new IngresParameter("cod_orgao", autorizacao.OrgaoAutorizado.Codigo));

                comando.ExecuteNonQuery();*/
            }

            Aplicativo app = null;


            try
            {

                app = autorizacao.Aplicativo;
                foreach (Menu menu in app.Menus)
                {
                    foreach (SubMenu submen in menu.SubMenus)
                    {
                        sql = "insert into sis_rel_usuario_submenu( " +
                                            "cod_orgao, " + // 0
                                            "cod_usuari, " + // 1
                                            "nom_aplica, " + // 2
                                            "nom_menu, " + // 3
                                            "nom_submen, " + // 4
                                            "cod_usuari_respon_libera, " + // 5
                                            "dat_libera, " +
                                            "des_observ_libera, " + // 6
                                            "ide_autori_inclui, " + // 7
                                            "ide_autori_altera, " + // 8
                                            "ide_autori_exclui) " + // 9
                                      "Values(?, " + // cod_orgao
                                             "?, " + // cod_usuari
                                             "?, " + // nom_aplica
                                             "?, " + // nom_menu
                                             "?, " + // nom_submen
                                             "?, " + // cod_usuari_respon_libera
                                             "DATE('NOW'), " +
                                             "?, " + // des_observ_libera
                                             "?, " + // ide_autori_inclui
                                             "?, " + // ide_autori_altera
                                             "?) ";  // ide_autori_exclui
                        comando = new SqlCommand(sql, conexao, transacao);

                        comando.Parameters.Add(new SqlParameter("cod_orgao",
                                            autorizacao.OrgaoAutorizado.Codigo));
                        comando.Parameters.Add(new SqlParameter("cod_usuari",
                                                        autorizacao.Usuario.Codigo.ToLower()));
                        comando.Parameters.Add(new SqlParameter("nom_aplica", app.Nome));
                        comando.Parameters.Add(new SqlParameter("nom_menu", menu.Nome));
                        comando.Parameters.Add(new SqlParameter("nom_submen", submen.Nome));
                        comando.Parameters.Add(new SqlParameter("cod_usuari_respon_libera",
                                            autorizacao.UsuarioAutorizando.Codigo));
                        comando.Parameters.Add(new SqlParameter("des_observ_libera",
                                            TratarNulidade(submen.Liberacao.Observacao)));
                        comando.Parameters.Add(new SqlParameter("ide_autori_inclui",
                                            submen.Liberacao.Permissoes.PermissaoIncluir == true ? "S" : "N"));
                        comando.Parameters.Add(new SqlParameter("ide_autori_altera",
                                            submen.Liberacao.Permissoes.PermissaoAlterar == true ? "S" : "N"));
                        comando.Parameters.Add(new SqlParameter("ide_autori_exclui",
                                            submen.Liberacao.Permissoes.PermissaoExcluir == true ? "S" : "N"));

                        comando.ExecuteNonQuery(); // executar insert
                    } // foreach Submenu
                } // foreach Menu
                return;
            }
            catch (SqlException e)
            {
                app = autorizacao.Aplicativo;
            }

            foreach (Menu menu in app.Menus)
            {
                foreach (SubMenu submen in menu.SubMenus)
                {

                    sql = "UPDATE sis_rel_usuario_submenu set " +
                                 "des_observ_libera = ?, " +
                                 "ide_autori_inclui = ?, " +
                                 "ide_autori_altera = ?, " +
                                 "ide_autori_exclui = ?, " +
                                 "dat_fim_libera = null  " +
                          "where cod_orgao = ?  and " +
                                "cod_usuari = ? and " +
                                "nom_aplica = ? and " +
                                "nom_menu = ? and " +
                                "nom_submen = ? ";

                    comando = new SqlCommand(sql, conexao, transacao);
                    comando.Parameters.Add(new SqlParameter("des_observ_libera",
                                           TratarNulidade(submen.Liberacao.Observacao)));

                    comando.Parameters.Add(new SqlParameter("ide_autori_inclui",
                                          submen.Liberacao.Permissoes.PermissaoIncluir == true ? "S" : "N"));

                    comando.Parameters.Add(new SqlParameter("ide_autori_altera",
                                          submen.Liberacao.Permissoes.PermissaoAlterar == true ? "S" : "N"));

                    comando.Parameters.Add(new SqlParameter("ide_autori_exclui",
                                          submen.Liberacao.Permissoes.PermissaoExcluir == true ? "S" : "N"));

                    comando.Parameters.Add(new SqlParameter("cod_orgao",
                                          autorizacao.OrgaoAutorizado.Codigo));

                    comando.Parameters.Add(new SqlParameter("cod_usuari",
                                          autorizacao.Usuario.Codigo));

                    comando.Parameters.Add(new SqlParameter("nom_aplica",
                                          app.Nome));

                    comando.Parameters.Add(new SqlParameter("nom_menu",
                                          menu.Nome));

                    comando.Parameters.Add(new SqlParameter("nom_submen",
                                          submen.Nome));

                    comando.ExecuteNonQuery();  // executar UPDATE

                }// foreach SubMenu

            }// foreach Menu

        }

        public override void Excluir(object entidade)
        {
            Autorizacao auth = (Autorizacao)entidade;
            string sql;

            // desautorizar todas as autorizações do órgão informado
            sql = "update sis_rel_usuario_submenu " +
                  "set    dat_fim_libera = Date('Now') " +
                  "where  cod_usuari = ? and " +
                         "cod_orgao = ? ";

            comando = new SqlCommand(sql, conexao, transacao);
            comando.Parameters.Add(new SqlParameter("cod_usuari", auth.Usuario.Codigo));
            comando.Parameters.Add(new SqlParameter("cod_orgao", auth.OrgaoAutorizado.Codigo));

            comando.ExecuteNonQuery();

        }

        public override IList<object> Consultar(object entidade)
        {
            Autorizacao autorizacao = (Autorizacao)entidade;
            IList<object> retorno = new List<object>();
            string sql;

            if (autorizacao.Aplicativo == null)
                return BuscaAutorizacoesDoUsuario(autorizacao, retorno);


            sql = "select nom_aplica " +
                "from   sis_rel_usuario_submenu " +
                "where  lower(cod_usuari) = lower(?) " +
                  "AND  cod_orgao = ? " +
                  "AND  nom_aplica = ? " +
                  "AND  nom_menu = ? " +
                  "AND  nom_submen = ? " +
                  "AND  (dat_inicio_libera is null " +
                        "OR Date('Now') > dat_inicio_libera) " +
                  "AND  (dat_fim_libera is null " +
                        "OR Date('Now') < dat_fim_libera) ";


            comando = new SqlCommand(sql, conexao, transacao);

            Aplicativo app = autorizacao.Aplicativo;

            // colocar parâmetros na consulta
            comando.Parameters.Add(new SqlParameter("cod_usuari", autorizacao.Usuario.Codigo));
            comando.Parameters.Add(new SqlParameter("cod_orgao",
                                        autorizacao.OrgaoAutorizado.Codigo));
            comando.Parameters.Add(new SqlParameter("nom_aplica",
                                            app.Nome));
            comando.Parameters.Add(new SqlParameter("nom_menu",
                                        app.Menus[0].Nome));
            comando.Parameters.Add(new SqlParameter("nom_submen",
                                        app.Menus[0].SubMenus[0].Nome));

            dataReader = comando.ExecuteReader();
            if (dataReader.Read())
            {
                retorno.Add(autorizacao);
            } // if
            dataReader.Close();



            if (retorno.Count == 0)
                retorno = null;
            return retorno;
        }

        /// <summary>
        /// Método private que seleciona todas as autorizações de um usuário.
        /// Se o perfil do usuário que estiver autorizando for de Autorizador, ele
        /// trará apenas os métodos do órgão atualmente logado
        /// </summary>
        /// <param name="auth">autorização recebida pelo consultar</param>
        /// <param name="retornoAutorizacoes">Lista instanciada no consultar</param>
        /// <returns></returns>
        private IList<object> BuscaAutorizacoesDoUsuario(Autorizacao auth, IList<object> retorno)
        {
            string sql;

            sql = "select sr.cod_usuari, " +   // 0 
                         "o.sig_orgao, " +   // 1
                         "ss.des_submen, " +   // 2
                         "sr.des_observ_libera, " +   // 3
                         "sr.ide_autori_inclui, " +   // 4
                         "sr.ide_autori_altera, " +   // 5
                         "sr.ide_autori_exclui, " +   // 6
                         "o.cod_orgao, " +   // 7 
                         "o.des_orgao, " +  // 8
                         "ss.nom_aplica, " + // 9
                         "ss.nom_menu, " + // 10
                         "ss.nom_submen " + // 11
                  "from   sis_rel_usuario_submenu sr, " +
                         "orgao o, " +
                         "sis_submenu_aplicativo ss " +
                  "where  sr.cod_usuari = ? and " +
                         "sr.cod_orgao = o.cod_orgao and " + // Join entre orgao e sis_rel_usuario_submenu
                         "sr.nom_aplica = ss.nom_aplica and " + // Join entre 
                         "sr.nom_menu = ss.nom_menu and " + // sis_rel_usuario_submenu e
                         "sr.nom_submen = ss.nom_submen and " + // sis_submenu_aplicativo                         
                         "(sr.dat_fim_libera is null or " +
                                "sr.dat_fim_libera > Date('Now')) and " +
                         "(sr.dat_inicio_libera is null or " +
                                "sr.dat_inicio_libera <= Date('Now')) ";

            if (auth.UsuarioAutorizando.Perfil != Perfil.GERENTE)
                sql += "and sr.cod_orgao = ? ";


            sql += "order by o.sig_orgao, ss.nom_aplica, ss.des_submen";

            comando = new SqlCommand(sql, conexao, transacao);
            comando.Parameters.Add(new SqlParameter("cod_usuari", auth.Usuario.Codigo)); // sr.cod_usuari


            if (auth.UsuarioAutorizando.Perfil != Perfil.GERENTE)
            {
                comando.Parameters.Add(new SqlParameter("cod_orgao", // cod_orgao
                                                        auth.UsuarioAutorizando.OrgaoAtual.Codigo));

            }

            dataReader = comando.ExecuteReader();
            while (dataReader.Read())
            {
                Autorizacao item = new Autorizacao();

                item.Usuario = new Profissional();
                item.Usuario.Codigo = dataReader.GetString(0); // cod_usuari
                item.UsuarioAutorizando = auth.UsuarioAutorizando; // perfil do autorizador

                item.OrgaoAutorizado = new Orgao();
                item.OrgaoAutorizado.Sigla = dataReader.GetString(1); // sig_orgao
                item.OrgaoAutorizado.Codigo = dataReader.GetString(7); // cod_orgao
                item.OrgaoAutorizado.Descricao = dataReader.GetString(8); // des_orgao

                item.Aplicativo = new Aplicativo
                {
                    Menus = new List<Menu>(),
                    Nome = dataReader.GetString(9), // nom_aplica
                };
                item.Aplicativo.Menus.Add(new Menu
                {
                    SubMenus = new List<SubMenu>(),
                    Nome = dataReader.GetString(10), // nom_menu
                });
                item.Aplicativo.Menus[0].SubMenus.Add(new SubMenu
                {
                    Descricao = dataReader.GetString(2), // des_submen
                    Nome = dataReader.GetString(11), // nom_submen
                });

                item.Aplicativo.Menus[0].SubMenus[0].Liberacao = new Liberacao
                {
                    Permissoes = new Permissoes
                    {
                        PermissaoIncluir = dataReader.GetString(4) == "S", // ide_autori_inclui
                        PermissaoAlterar = dataReader.GetString(5) == "S", // ide_autori_altera
                        PermissaoExcluir = dataReader.GetString(6) == "S", // ide_autori_exclui
                    },
                    Observacao = (string)ObjetoOuNulo(3) // des_observ_libera
                };

                retorno.Add(item);

            }
            dataReader.Close();

            if (retorno.Count == 0)
                return null;
            return retorno;

        }
    }
}

