using Crud_Facade_Acesso.Dados.Web.Contexto;
using Crud_Facade_Modelos.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Acesso.Servicos.Projeto.Web.Contexto
{
    public class ContextoCopiaDeAutorizacao : ContextoSql
    {
        public override void Salvar(object entidade)
        {
            CopiaDeAutorizacao copia = entidade as CopiaDeAutorizacao;

            copia.AutorizacaoDestino.Aplicativo = copia.AutorizacaoOrigem.Aplicativo;
            copia.AutorizacaoDestino.OrgaoAutorizado = copia.AutorizacaoOrigem.OrgaoAutorizado;


            IContextoDAO dao = new ContextoAutorizacao();
            dao.CompartilhaConexao(this.RetornaConexao());
            dao.CompartilharTransacao(this.RetornaTransacao());

            dao.Salvar(copia.AutorizacaoDestino);

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
            CopiaDeAutorizacao copia = entidade as CopiaDeAutorizacao;
            string sql;

            IContextoDAO dao = new ContextoAutorizacao();
            dao.CompartilhaConexao(this.RetornaConexao()); // para manter a mesma conexão
            dao.CompartilharTransacao(this.RetornaTransacao()); // para manter a mesma transação

            IList<Autorizacao> retornoAutorizacoes;
            var auxiliar = dao.Consultar(copia.AutorizacaoOrigem);

            if (auxiliar != null)
                retornoAutorizacoes = auxiliar.OfType<Autorizacao>().ToList();
            else
                return null;

            IList<object> ListaDeCopias = new List<object>();
            IList<string> siglasOrgaos = new List<string>();

            // Selecionar o órgão padrão: 
            sql = "select sig_orgao, " +    // 0
                         "o.cod_orgao " +   // 1
                  "from   orgao o, " +
                         "usuario u " +
                  "where  o.cod_orgao = u.cod_orgao_padrao and " +
                         "u.cod_usuari = ? ";

            comando = new SqlCommand(sql, conexao, transacao);
            comando.Parameters.Add(new SqlParameter("cod_usuari",
                                copia.AutorizacaoDestino.Usuario.Codigo));

            dataReader = comando.ExecuteReader();
            if (dataReader.Read())
            {
                Profissional usuarioAuxiliar = copia.AutorizacaoDestino.Usuario;
                usuarioAuxiliar.OrgaosDoUsuario = new List<Orgao>();

                siglasOrgaos.Add(dataReader.GetString(0)); // sig_orgao

                usuarioAuxiliar.OrgaosDoUsuario.Add(new Orgao
                {
                    Sigla = siglasOrgaos.Last(), // sig_orgao
                    Codigo = dataReader.GetString(1), // cod_orgao
                });

            }
            dataReader.Close();

            // selecionar os outros órgãos: 
            sql = "select sig_orgao " + // 0
                  "from   orgao o, " +
                         "rel_usuario_orgao r " +
                  "where  o.cod_orgao = r.cod_orgao and " +
                         "r.cod_usuari = ? and " +
                         "o.cod_orgao <> ? ";

            comando = new SqlCommand(sql, conexao, transacao);
            comando.Parameters.Add(new SqlParameter("cod_usuari", // cod_usuari
                                copia.AutorizacaoDestino.Usuario.Codigo));
            comando.Parameters.Add(new SqlParameter("cod_orgao",
                                copia.AutorizacaoDestino.Usuario.OrgaosDoUsuario[0].Codigo));


            dataReader = comando.ExecuteReader();
            while (dataReader.Read())
            {
                Profissional usuarioAuxiliar = copia.AutorizacaoDestino.Usuario;

                siglasOrgaos.Add(dataReader.GetString(0)); // sig_orgao

                usuarioAuxiliar.OrgaosDoUsuario.Add(new Orgao
                {
                    Sigla = siglasOrgaos.Last(), // sig_orgao
                });
            } // while
            dataReader.Close();

            retornoAutorizacoes = (from Autorizacao a in retornoAutorizacoes
                                   where siglasOrgaos.Contains(a.OrgaoAutorizado.Sigla)
                                   select a).ToList();

            foreach (Autorizacao a in retornoAutorizacoes)
            {
                CopiaDeAutorizacao retornoFinal = new CopiaDeAutorizacao();

                retornoFinal.AutorizacaoOrigem = new Autorizacao
                {
                    Usuario = copia.AutorizacaoOrigem.Usuario,
                    UsuarioAutorizando = copia.AutorizacaoOrigem.UsuarioAutorizando,
                    OrgaoAutorizado = a.OrgaoAutorizado,
                    Aplicativo = a.Aplicativo,
                };

                retornoFinal.AutorizacaoDestino = new Autorizacao
                {
                    Usuario = copia.AutorizacaoDestino.Usuario,
                    UsuarioAutorizando = copia.AutorizacaoDestino.UsuarioAutorizando,
                };

                ListaDeCopias.Add(retornoFinal); // salvando cópia na lista
            }

            if (ListaDeCopias.Count == 0)
                return null;
            return ListaDeCopias;
        }
    }
}

