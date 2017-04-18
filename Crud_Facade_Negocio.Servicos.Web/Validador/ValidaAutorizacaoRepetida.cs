using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crud_Facade_Modelos.Web;
using Crud_Facade_Negocios.Base.Web.Interfaces;
using Crud_Facade_Negocios.Servicos.Web.Fachada;

namespace Crud_Facade_Negocios.Servicos.Web.Validador
{
    /// <summary>
    /// Valida se a autorização já não foi dada para o usuário informado no órgão informado.
    /// </summary>
    public class ValidaAutorizacaoRepetida : ValidadorAbstrato
    {

        private IList<string> autorizacoesAnteriores;

        public ValidaAutorizacaoRepetida()
        {
            autorizacoesAnteriores = new List<string>();
        }

        public override string Executar(object entidade)
        {
            Autorizacao autorizacao = (Autorizacao)entidade;
            IList<Autorizacao> retorno;
            IFachada<Autorizacao> fachada = new FachadaAdmWeb<Autorizacao>();
            fachada.SalvaConexaoAtiva(this.conexao); // Manter conexão anterior
            fachada.SalvaTransacaoAtiva(this.transacao); // Manter transação anterior
            fachada.DefineTemQueFecharConexao(false); // Não fechar ao finalizar

            string rotina = autorizacao.Aplicativo.Menus[0].SubMenus[0].Descricao;

            if (autorizacoesAnteriores.Contains(rotina))
                return "Autorização em rotinas repetidas";

            autorizacoesAnteriores.Add(rotina);

            retorno = fachada.Consultar(autorizacao);

            if (retorno != null)
            {
                string mensagem = "Rotinas já autorizados previamente: \n";
                mensagem += "Usuário: " + retorno[0].Usuario.Codigo;
                mensagem += " no órgão: " + retorno[0].OrgaoAutorizado.Sigla + " \n";

                Aplicativo app = retorno[0].Aplicativo;

                mensagem += rotina;


                return mensagem; // retorna o erro

            }


            return null;
        }
    }
}

