using Crud_Facade_Modelos.Web;
using Crud_Facade_Modelos.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Acesso.Servicos.Projeto.Web.Contexto
{
    public class ContextoAutorizacaoViewModel : ContextoSql
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

            AutorizarUsuarioViewModel model = entidade as AutorizarUsuarioViewModel;
            string sql = "";
            IList<object> retorno = new List<object>();
            retorno.Add(model);

            if (model.UsuarioLiberacao.Perfil == Perfil.GERENTE) // é gerente??
            {
                // selecionar todas as rotinas disponíveis
                sql = "select des_submen " + // 0
                      "from   sis_submenu_aplicativo " +
                      "order by des_submen";
                comando = new SqlCommand(sql, conexao, transacao);
            }
            else // Perfil de autorizador: 
            {
                // selecionar apenas rotinas que ele tenha permissão
                sql = "select ss.des_submen " + // 0
                      "from   sis_submenu_aplicativo ss, " +
                             "sis_rel_usuario_submenu rs " +
                      "where  rs.cod_orgao = ? and " +
                             "rs.cod_usuari = ? and " +
                             "rs.nom_aplica = ss.nom_aplica and " +
                             "rs.nom_menu = ss.nom_menu and " +
                             "rs.nom_submen = ss.nom_submen and " +
                             "(rs.dat_fim_libera is null or " +
                                    "rs.dat_fim_libera >= Date('Now')) and " +
                             "(rs.dat_inicio_libera is null or " +
                                    "rs.dat_inicio_libera <= Date('Now')) " +
                      "order by des_submen";
                comando = new SqlCommand(sql, conexao, transacao);
                comando.Parameters.Add(new SqlParameter("cod_orgao", // cod_orgao
                                                model.UsuarioLiberacao.OrgaoAtual.Codigo));
                comando.Parameters.Add(new SqlParameter("cod_usuari", // cod_usuari
                                                model.UsuarioLiberacao.Codigo));

            }

            dataReader = comando.ExecuteReader();
            model.RotinasDisponíveis = new List<string>();
            while (dataReader.Read())
            {
                model.RotinasDisponíveis.Add(dataReader.GetString(0)); // des_submen
            }
            dataReader.Close(); // fechar para usar depois

            return retorno;

        }
    }
}
