using Crud_Facade_Modelos.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Acesso.Servicos.Projeto.Web.Contexto
{
    public class ContextoOrgao : ContextoSql
    {
        public override void Salvar(object entidade)
        {

            throw new NotImplementedException("Método indisponível para a entidade Órgão");


        }


        public override void Alterar(object entidade)
        {
            throw new NotImplementedException("Método indisponível para a entidade Órgão");

        }

        public override void Excluir(object entidade)
        {
            throw new NotImplementedException("Método indisponível para a entidade Órgão");

        }

        public override IList<object> Consultar(object entidade)
        {
            Orgao orgao = entidade as Orgao;
            string sql = "select cod_orgao, " + // 0
                                "sig_orgao  " + // 1
                         "from   orgao " +
                         "where  upper(cod_orgao) = upper(?) Or " +//cod_orgao
                                "upper(sig_orgao) = upper(?)";//sig_orgao
            comando = new SqlCommand(sql, conexao, transacao);
            comando.Parameters.Add(new SqlParameter("cod_orgao", orgao.Codigo.Trim()));
            comando.Parameters.Add(new SqlParameter("sig_orgao", orgao.Sigla.Trim()));

            dataReader = comando.ExecuteReader(); //executar read

            if (dataReader.Read())
            {
                Orgao novo = new Orgao();
                novo.Codigo = dataReader.GetString(0);
                novo.Sigla = dataReader.GetString(1);
                dataReader.Close();

                IList<object> retorno = new List<object>();
                retorno.Add(novo);

                dataReader.Close(); // para poder usar novamente mais tarde
                return retorno;
            }
            dataReader.Close(); // para poder usar novamente mais tarde
            return null;

        }
    }
}
