using System;
using Crud_Facade_Modelos.Web;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Acesso.Servicos.Projeto.Web.Contexto
{
    public class ContextoUsuario : ContextoSql
    {
        public override void Salvar(object entidade)
        {
            Profissional usuario = (Profissional)entidade;
            string sql;
            sql = "insert into usuario(" +
                  "cod_usuari," +// 0
                  "nom_usuari," +// 1
                  "sen_usuari," +// 2
                  "cod_orgao_padrao," +// 3
                  "cod_nivel_usuari)" +// 4
                  "Values ( ?," +//cod_usuari
                           "?," +//nom_usuari
                           "?," +//sen_usuari
                           "?," +//cod_orgao_padrao
                           "'N') ";//cod_nivel_usuari

            comando = new SqlCommand(sql, conexao, transacao);
            comando.Parameters.Add(new SqlParameter("cod_usuari",
                                 usuario.Codigo.ToLower()));
            comando.Parameters.Add(new SqlParameter("nom_usuari",
                                usuario.Nome.ToUpper().Trim()));
            comando.Parameters.Add(new SqlParameter("sen_usuari",
                                usuario.Codigo.Trim()));
            comando.Parameters.Add(new SqlParameter("cod_orgao_padrao",
                          usuario.OrgaoPadrao.Codigo.Trim()));
            //comando.Parameters.Add(new IngresParameter("cod_nivel_usuari",
            comando.ExecuteNonQuery();

            foreach (Orgao org in usuario.OrgaosDoUsuario)
            {
                sql = "insert into rel_usuario_orgao( " +
                           "cod_orgao," +            // 0
                           "cod_usuari, " +          // 1
                           "cod_impres_padrao, " +
                           "tip_saida_padrao, " +
                           "sin_distri_instan) " +
                       "Values(?, " +//cod_orgao
                              "?, " +//cod_usuari
                              "'elgin', " +
                              "'T', " +
                              "'N')";

                comando = new SqlCommand(sql, conexao, transacao);

                comando.Parameters.Add(new SqlParameter("cod_orgao",
                                   org.Codigo));
                comando.Parameters.Add(new SqlParameter("cod_usuari",
                                   usuario.Codigo.ToLower()));


                comando.ExecuteNonQuery();

            }

        }


        public override void Alterar(object entidade)
        {
            Profissional usuario = (Profissional)entidade;

            string sql;
            sql = "update usuario set(" +
                  "nom_usuari =?" +// 0
                  "where cod_usuari=?)";
            comando = new SqlCommand(sql, conexao, transacao);
            comando.Parameters.Add(new SqlParameter("nom_usuari",
                                usuario.Nome.ToUpper().Trim()));
            comando.Parameters.Add(new SqlParameter("cod_usuari",
                                 usuario.Codigo.ToLower()));

            comando.ExecuteNonQuery();

            foreach (Orgao org in usuario.OrgaosDoUsuario)
            {
                sql = "update rel_usuario_orgao set( " +
                           "cod_orgao =?," +
                           "cod_usuari =?," +
                           "cod_impres_padrao =?," +
                           "tip_saida_padrao =?," +
                           "sin_distri_instan =?" +
                           "where cod_orgao =?";
                comando = new SqlCommand(sql, conexao, transacao);

                comando.Parameters.Add(new SqlParameter("cod_orgao",
                                 org.Codigo));
                comando.Parameters.Add(new SqlParameter("cod_usuari",
                                 usuario.Codigo.ToLower()));


                comando.ExecuteNonQuery();

            }
        }

        public override void Excluir(object entidade)
        {

            throw new NotImplementedException();
        }


        public override IList<object> Consultar(object entidade)
        {
            Profissional usuario = entidade as Profissional;

            string sql;
            IList<object> retorno = new List<object>();

            if (usuario.OrgaoAtual != null && !string.IsNullOrEmpty(usuario.OrgaoAtual.Codigo))
            {
                sql = "select cod_orgao " +
                      "from   rel_usuario_orgao " +
                      "where  cod_orgao = ? and " +
                             "lower(cod_usuari) = lower(?) ";
                comando = new SqlCommand(sql, conexao, transacao);
                comando.Parameters.Add(new SqlParameter("cod_orgao", usuario.OrgaoAtual.Codigo));
                comando.Parameters.Add(new SqlParameter("cod_usuario", usuario.Codigo));

                dataReader = comando.ExecuteReader();

                if (dataReader.Read())
                {
                    retorno.Add(usuario);
                    dataReader.Close();
                    return retorno;
                }

                dataReader.Close();

                sql = "select cod_orgao_padrao " +
                      "from   usuario " +
                      "where  cod_orgao_padrao = ? and " +
                             "lower(cod_usuari) = lower(?) ";
                comando = new SqlCommand(sql, conexao, transacao);
                comando.Parameters.Add(new SqlParameter("cod_orgao", usuario.OrgaoAtual.Codigo));
                comando.Parameters.Add(new SqlParameter("cod_usuario", usuario.Codigo));

                dataReader = comando.ExecuteReader();

                if (dataReader.Read())
                {
                    retorno.Add(usuario);
                }

                dataReader.Close();
            }
            else
            {
                sql = "select cod_usuari " +
                      "from usuario " +
                      "where cod_usuari = ? ";
                comando = new SqlCommand(sql, conexao, transacao);
                comando.Parameters.Add(new SqlParameter("cod_usuari",
                                       usuario.Codigo.ToUpper()));
                dataReader = comando.ExecuteReader(); //executar read

                if (dataReader.Read())
                {
                    retorno.Add(usuario);
                }
            }
            dataReader.Close();

            if (retorno.Count == 0)
                retorno = null;
            return retorno;

        }
    }
}
