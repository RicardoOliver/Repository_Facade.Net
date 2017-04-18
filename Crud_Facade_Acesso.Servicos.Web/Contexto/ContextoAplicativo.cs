using Crud_Facade_Modelos.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Acesso.Servicos.Projeto.Web.Contexto
{

    public class ContextoAplicativo : ContextoSql
    {

        public override void Salvar(object entidade)
        {
            Aplicativo aplicativo = (Aplicativo)entidade;
            string sql;

            sql = "insert into sis_aplicativo(" +
                 " nom_Aplica," +//0 
                 " des_aplica," +//1 
                 " des_sintet_aplica," +//2 
                 " cod_versao_aplica," +//3 
                 " nom_progra_aplica," +//4 
                 "  ide_plataf_aplica," +//5 
                 " nom_progra_aplica_logoff, " +//6 
                 " cod_projet_bcs ) " +
                 " Values (?," +//Nome
                          "?," +//Descricao 
                          "?," +//DescricaoSintetica 
                          "?," +//Versao
                          "?," +//URLDeExecucao
                          "?," +//Plataforma 
                          "?)";//URLDeLogoff
            comando = new SqlCommand(sql, conexao, transacao);

            comando.Parameters.Add(new SqlParameter("nom_Aplica",
                                  aplicativo.Nome.ToUpper()));
            comando.Parameters.Add(new SqlParameter("des_aplica",
                                        aplicativo.Descricao));
            comando.Parameters.Add(new SqlParameter("des_sintet_aplica",
                               aplicativo.DescricaoSintetica));
            comando.Parameters.Add(new SqlParameter("cod_versao_aplica",
                                           aplicativo.Versao));
            comando.Parameters.Add(new SqlParameter("nom_progra_aplica",
                                    aplicativo.URLDeExecucao));
            comando.Parameters.Add(new SqlParameter("ide_plataf_aplica",
                             aplicativo.Plataforma.ToUpper()));
            comando.Parameters.Add(new SqlParameter("nom_progra_aplica_logoff",
                                      aplicativo.URLDeLogoff));


            comando.ExecuteNonQuery(); //Executar insert


            foreach (Menu m in aplicativo.Menus)
            {
                sql = "insert into sis_menu_aplicativo(" +
                       "Nom_aplica, " +//0
                       "Nom_Menu, " +//1
                       "des_menu, " +//2
                       "num_posica_menu) " +//3
                       "values( ?, " +//Nom_aplica
                               "?,  " +//Nom_Menu
                               "?, " +//Des_Menu
                               "?) ";//Num_posica_Menu
                comando = new SqlCommand(sql, conexao, transacao);
                comando.Parameters.Add(new SqlParameter("Nom_aplica",
                                   aplicativo.Nome.ToUpper()));
                comando.Parameters.Add(new SqlParameter("Nom_Menu",
                                   m.Nome.ToUpper()));
                comando.Parameters.Add(new SqlParameter("des_menu",
                                  m.Descricao.Trim()));
                comando.Parameters.Add(new SqlParameter("num_posica_menu",
                              m.Posicao));


                comando.ExecuteNonQuery();


                foreach (SubMenu sub in m.SubMenus)
                {
                    sql = "insert into sis_submenu_aplicativo(" +
                           "Nom_aplica, " +//0
                           "Nom_menu, " +//1
                           "Nom_Submen, " +//2
                           "des_submen, " +//3
                           "nom_proced_SubMen, " +//4
                           "num_posica_submen, " +//5
                           "ide_divisa_submen ) " +//6
                           "values(?, " +//Nom_aplica
                                  "?, " +//nom_menu
                                  "?, " +//Nom_SubMen
                                  "?, " +//des_submen
                                  "?, " +//nom_proced_subMen
                                  "?, " +//ide_divisa_submen
                                  "'N') ";
                    comando = new SqlCommand(sql, conexao, transacao);
                    comando.Parameters.Add(new SqlParameter("Nom_aplica",
                                      aplicativo.Nome.ToUpper()));
                    comando.Parameters.Add(new SqlParameter("Nom_menu",
                                    m.Nome.ToUpper()));
                    comando.Parameters.Add(new SqlParameter("Nom_Submen",
                                       sub.Nome.ToUpper()));
                    comando.Parameters.Add(new SqlParameter("des_submen",
                                       sub.Descricao.Trim()));
                    comando.Parameters.Add(new SqlParameter("nom_proced_SubMenu",
                                        sub.ActionSubMenu.Trim()));
                    comando.Parameters.Add(new SqlParameter("num_posica_submen",
                                         sub.Posicao));


                    comando.ExecuteNonQuery();


                }//foreach Menu      

            }
        }


        public override void Alterar(object entidade)
        {
            Aplicativo aplicativo = (Aplicativo)entidade;
            string sql;
            sql = "UPDATE sis_aplicativo SET " +
                                 "des_aplica = ?," +//0
                                 "des_sintet_aplica = ?," +//1
                                 "nom_progra_aplica = ?," +//2
                                 "nom_progra_aplica_logoff = ?" +//3
                                 "where nom_aplica = ?";//4
            comando = new SqlCommand(sql, conexao, transacao);
            comando.Parameters.Add(new SqlParameter("des_aplica",
                                                      aplicativo.Descricao.Trim()));//des_aplica
            comando.Parameters.Add(new SqlParameter("des_sintet_aplica",
                                                      aplicativo.DescricaoSintetica.Trim()));//des_sintet_aplica
            comando.Parameters.Add(new SqlParameter("nom_progra_aplica",
                                                      aplicativo.URLDeExecucao.Trim()));//nom_progra_aplica
            comando.Parameters.Add(new SqlParameter("nom_progra_aplica_logoff",
                                                     aplicativo.URLDeLogoff.Trim()));//URLDeLogoff
            comando.Parameters.Add(new SqlParameter("nom_aplica",
                                                      aplicativo.Nome.ToUpper()));//nom_Aplica


            comando.ExecuteNonQuery();//Executar update



            foreach (Menu m in aplicativo.Menus)
            {
                sql = "UPDATE sis_menu_aplicativo SET " +
                        "nom_menu = ?, " +//0
                        "des_menu = ?" + //1
                        "where nom_aplica = ? ";//2
                comando = new SqlCommand(sql, conexao, transacao);
                comando.Parameters.Add(new SqlParameter("nom_menu",
                                      m.Nome.Trim()));//nom_menu
                comando.Parameters.Add(new SqlParameter("des_menu",
                                   m.Descricao.Trim())); //des_menu
                comando.Parameters.Add(new SqlParameter("nom_aplica",
                                  aplicativo.Nome.ToUpper()));//nom_aplica



                comando.ExecuteNonQuery();//Executar update



                foreach (SubMenu sub in m.SubMenus)
                {
                    sql = "UPDATE sis_submenu_aplicativo SET " +
                           "des_submen = ? " +//0
                           "where nom_aplica = ? ";//1
                    comando = new SqlCommand(sql, conexao, transacao);
                    comando.Parameters.Add(new SqlParameter("des_submen",
                                                 sub.Descricao.Trim()));//des_submen
                    comando.Parameters.Add(new SqlParameter("nom_aplica",
                                              aplicativo.Nome.ToUpper()));//Nom_aplica


                    comando.ExecuteNonQuery();//Executar update

                }
            }
        }


        public override void Excluir(object entidade)
        {
            Aplicativo aplicativo = (Aplicativo)entidade;
            string sql;
            sql = "delete * from  sis_aplicativo " +
                          " where nom_Aplica = ?," +//0
                           "des_aplica = ?,";//1
            comando = new SqlCommand(sql, conexao, transacao);
            comando.Parameters.Add(new SqlParameter("nom_Aplica",
                                       aplicativo.Nome.ToUpper()));//nom_Aplica
            comando.Parameters.Add(new SqlParameter("des_aplica",
                                   aplicativo.Descricao.ToLower())); //des_aplica




            comando.ExecuteNonQuery(); //Executar delete


            foreach (Menu m in aplicativo.Menus)
            {
                sql = "delete from sis_menu_aplicativo" +//0
                               "where nom_aplica = ?," +//1
                                      "nom_menu  = ?," +//2
                                       "des_menu  = ? ";//3
                comando = new SqlCommand(sql, conexao, transacao);
                comando.Parameters.Add(new SqlParameter("nom_aplica",
                                                   m.Nome.ToUpper()));//nom_aplica
                comando.Parameters.Add(new SqlParameter("nom_menu",
                                                    m.Nome.ToUpper()));//nom_menu
                comando.Parameters.Add(new SqlParameter("des_menu",
                                                   m.Descricao.Trim()));//des_menu


                comando.ExecuteNonQuery();// Executar delete


                foreach (SubMenu sub in m.SubMenus)
                {
                    sql = " delete from sis_submenu_aplicativo " +
                                       "where nom_aplica  = ?," +//0
                                               "nom_submen =?," +//1
                                                 "nom_menu  = ?";//2
                    comando = new SqlCommand(sql, conexao, transacao);
                    comando.Parameters.Add(new SqlParameter("nom_aplica",
                                                      sub.Nome.ToUpper()));//nom_aplica
                    comando.Parameters.Add(new SqlParameter("nom_submen",
                                                      sub.Nome.ToUpper()));//nom_submen
                    comando.Parameters.Add(new SqlParameter("nom_menu",
                                                       m.Nome.ToUpper()));//nom_menu


                    comando.ExecuteNonQuery();// Executar delete

                }
            }
        }

        public override IList<object> Consultar(object entidade)
        {
            Aplicativo aplicativo = (Aplicativo)entidade;
            IList<object> retorno = new List<object>();
            // Aplicativo objRetorna = null;
            string sql;

            if (aplicativo.Nome == null || aplicativo.Nome.Trim() == "")
                return BuscaPorRotina(aplicativo);

            sql = " Select nom_aplica, " +           // 0 - Nome
                          "des_aplica, " +           // 1 - Descricao
                          "des_sintet_aplica, " +     // 2 - DescricaoSintetica
                          "nom_progra_aplica, " +     // 3 - URLDeExecucao                          
                          "cod_versao_aplica, " +      //4 - Versao
                          "nom_progra_aplica_logoff, " + // 5 - URLDeLogoff
                          "ide_plataf_aplica " +        // 6 - Plataforma
                       "FROM  Sis_Aplicativo   " + // Sis_Aplicativo
                       "Where Nom_Aplica = ? ";   // 0  


            comando = new SqlCommand(sql, conexao, transacao);

            //colocar parâmetros na consulta
            comando.Parameters.Add(new SqlParameter("Nome",
                                  aplicativo.Nome.ToUpper()));

            dataReader = comando.ExecuteReader();

            if (dataReader.Read())
            {
                Aplicativo app = new Aplicativo();

                app.Nome = dataReader.GetString(0); // 0 - nom_aplica

                app.Descricao = dataReader.GetString(1); // 1 - Descricao

                app.DescricaoSintetica = dataReader.GetString(2);// 2 - DescricaoSintetica

                app.URLDeExecucao = (string)ObjetoOuNulo(3); // 4 - URLDeexecucao

                app.Versao = (string)ObjetoOuNulo(4);  // 3 - cod_versao_aplica

                app.URLDeLogoff = (string)ObjetoOuNulo(5); // 6 - nom_progra_aplica_logoff

                app.Plataforma = dataReader.GetString(6); // 5 - Plataforma



                retorno.Add(app); // retornando o app
            }

            if (retorno.Count == 0)
                return null;
            return retorno;



        }

        private IList<object> BuscaPorRotina(Aplicativo aplicativo)
        {
            IList<object> retorno = new List<object>();
            string sql;

            sql = "select nom_aplica, " +   // 0 
                         "nom_menu, " +    // 1
                         "nom_submen " +   // 2
                  "from   sis_submenu_aplicativo " +
                  "where  des_submen = ? ";

            comando = new SqlCommand(sql, conexao, transacao);
            comando.Parameters.Add(new SqlParameter("des_submen",
                        aplicativo.Menus[0].SubMenus[0].Descricao));

            dataReader = comando.ExecuteReader();
            if (dataReader.Read())
            {
                aplicativo.Nome = dataReader.GetString(0); // nom_aplica
                aplicativo.Menus[0].Nome = dataReader.GetString(1); // nom_menu
                aplicativo.Menus[0].SubMenus[0].Nome = dataReader.GetString(2); // nom_submen
                retorno.Add(aplicativo);
            }
            dataReader.Close();

            if (retorno.Count == 0)
                retorno = null;
            return retorno;
        }
    }
}


// Consultar()


