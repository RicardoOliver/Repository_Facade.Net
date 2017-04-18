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
    /// Valida se uma rotina já foi informada mais de uma vez no mesmo órgão.
    /// Valida também se as rotinas informadas existem
    /// </summary>
    public class ValidaRotinasInformadas : ValidadorAbstrato
    {
        private IDictionary<string, IList<string>> valoresQueJaforam;


        public ValidaRotinasInformadas()
        {
            valoresQueJaforam = new Dictionary<string, IList<string>>();
        }

        public override string Executar(object entidade)
        {
            Autorizacao auth = entidade as Autorizacao;
            string chave = auth.OrgaoAutorizado.Sigla;
            Aplicativo a = auth.Aplicativo;

            if (!valoresQueJaforam.ContainsKey(chave))
                valoresQueJaforam.Add(auth.OrgaoAutorizado.Sigla, new List<string>());


            if (valoresQueJaforam[chave].Contains(a.Menus[0].SubMenus[0].Descricao)) // Rotina repetida??
                return "Rotina " + a.Menus[0].SubMenus[0].Descricao + " está informada repetidamente " +
                    "no Órgão " + auth.OrgaoAutorizado.Sigla;

            valoresQueJaforam[chave].Add(a.Menus[0].SubMenus[0].Descricao);

            IFachada<Aplicativo> fachada = new FachadaAdmWeb<Aplicativo>();
            fachada.SalvaConexaoAtiva(this.conexao); // Manter conexão anterior
            fachada.SalvaTransacaoAtiva(this.transacao); // Manter transação anterior
            fachada.DefineTemQueFecharConexao(false); // Não fechar ao finalizar

            IList<Aplicativo> retorno = fachada.Consultar(a);

            if (retorno == null)
                return "Rotina " + a.Menus[0].SubMenus[0].Descricao + " é inválida";





            return null;

        }
    }
}
