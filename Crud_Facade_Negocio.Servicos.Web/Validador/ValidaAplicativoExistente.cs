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
    /// Valida se um aplicativo já foi cadastrado na base de dados.
    /// </summary>
    public class ValidaAplicativoExistente : ValidadorAbstrato
    {
        public override string Executar(object entidade)
        {

            Aplicativo aplicativo = (Aplicativo)entidade;

            IFachada<Aplicativo> fachada = new FachadaAdmWeb<Aplicativo>();
            fachada.SalvaConexaoAtiva(this.conexao); // Manter conexão anterior
            fachada.SalvaTransacaoAtiva(this.transacao); // Manter transação anterior
            fachada.DefineTemQueFecharConexao(false); // Não fechar ao finalizar

            IList<Aplicativo> codigoAplicacaoRetonornado = fachada.Consultar(aplicativo);


            if (codigoAplicacaoRetonornado != null)//se não retornar null, é porque ocorreu um erro de validação
            {
                string retorna = "Aplicação já cadastrada.";
                return retorna;
            }

            return null;

        }
    }
}


