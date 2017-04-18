using Crud_Facade_Modelos.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Crud_Facade_Acesso.Servicos.Projeto.Web.Contexto
{
    public class ContextoSubMenusAutorizados : ContextoSql
    {
        public override void Salvar(object entidade)
        {
            throw new NotImplementedException();
        }

        public override void Alterar(object entidade)
        {
            throw new NotImplementedException();
        }

        public override void Excluir(object entidade)
        {
            throw new NotImplementedException();
        }

        public override IList<object> Consultar(object entidade)
        {
            SubMenu submenu = (SubMenu)entidade;
            IList<object> retorno = new List<object>();
            string sql;

            sql = "select ide_autori_inclui, " +   // 0
                         "ide_autori_altera, " +   // 1
                         "ide_autori_exclui, " +   // 2
                         "ss.des_submen " +   // 3
                  "from   sis_rel_usuario_submenu sr, " +
                         "sis_submenu_aplicativo ss " +
                  "where  sr.cod_usuari = ? and " +
                         "ss.nom_aplica = sr.nom_aplica and " +
                         "ss.nom_menu = sr.nom_menu and " +
                         "ss.nom_submen = sr.nom_submen";

            comando = new SqlCommand(sql, conexao, transacao);
            comando.Parameters.Add(new SqlParameter("cod_usuari",
                                    submenu.Liberacao.UsuarioLiberacao.Codigo));

            dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                SubMenu itemLiberacao = new SubMenu();

                itemLiberacao.Liberacao = new Liberacao();

                itemLiberacao.Liberacao.Permissoes = new Permissoes
                {
                    PermissaoIncluir = dataReader.GetString(0) == "S" ? true : false, // ide_autori_inclui
                    PermissaoAlterar = dataReader.GetString(1) == "S" ? true : false, // ide_autori_altera
                    PermissaoExcluir = dataReader.GetString(2) == "S" ? true : false, // ide_autori_exclui
                };

                itemLiberacao.Liberacao.UsuarioLiberacao = submenu.Liberacao.UsuarioLiberacao;

                itemLiberacao.Descricao = dataReader.GetString(3); // des_submen

                retorno.Add(itemLiberacao);
            }


            if (retorno.Count == 0)
                return null;
            return retorno;
        }
    }
}
