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
    /// Valida se as Permissões (incluir, alterar e excluir) são as mesmo do autorizados.
    /// Caso o perfil do usuário logado seja de gerente, o processo é ignorado retornando null.
    /// </summary>
    public class ValidaPermissoesAutorizacao : ValidadorAbstrato
    {
        public override string Executar(object entidade)
        {
            Autorizacao auth = (Autorizacao)entidade;
            SubMenu liberacao = auth.Aplicativo.Menus[0].SubMenus[0];

            if (liberacao.Liberacao.UsuarioLiberacao.Perfil == Perfil.GERENTE)
                return null; // não é necessário verificar


            if (auth == null)
                return "Autorização Inválida";

            IFachada<SubMenu> fachada = new FachadaAdmWeb<SubMenu>();
            fachada.SalvaConexaoAtiva(this.conexao); // Manter conexão anterior
            fachada.SalvaTransacaoAtiva(this.transacao); // Manter transação anterior
            fachada.DefineTemQueFecharConexao(false); // Não fechar ao finalizar

            IList<SubMenu> liberacoesDoLogado = fachada.Consultar(liberacao);

            if (liberacoesDoLogado == null)
                return "Permissões inválidas";

            IDictionary<string, SubMenu> MapaPermisoes = new Dictionary<string, SubMenu>();

            foreach (SubMenu s in liberacoesDoLogado)
                MapaPermisoes.Add(s.Descricao, s);

            Aplicativo a = auth.Aplicativo;

            // Verificar se as autorizações obedecem as permissões de inclusão, exclusão e alteração                        
            foreach (Menu m in a.Menus)
            {
                foreach (SubMenu s in m.SubMenus)
                {
                    if (!MapaPermisoes.ContainsKey(s.Descricao)) // permissão diferente do autorizador??
                        return "Somente podem ser dadas as mesmas permissões do autorizador.\n O " +
                                "autorizador não tem permissão de acesso à rotina " + s.Descricao;

                    SubMenu auxiliar = MapaPermisoes[s.Descricao];

                    string msgAuxiliar = "O autorizador só pode autorizar permissões que já tenha.\n";

                    if (auxiliar.Liberacao.Permissoes.PermissaoIncluir == false &&
                        s.Liberacao.Permissoes.PermissaoIncluir == true)
                        return msgAuxiliar + "O autorizador não tem permissão de inclusão na " +
                                "rotina " + s.Descricao;

                    if (auxiliar.Liberacao.Permissoes.PermissaoExcluir == false &&
                        s.Liberacao.Permissoes.PermissaoExcluir == true)
                        return msgAuxiliar + "O autorizador não tem permissão de exclusão na " +
                                "rotina " + s.Descricao;

                    if (auxiliar.Liberacao.Permissoes.PermissaoAlterar == false &&
                        s.Liberacao.Permissoes.PermissaoAlterar == true)
                        return msgAuxiliar + "O autorizador não tem permissão de alteração na " +
                                "rotina " + s.Descricao;

                } // foreach SubMenu

            } // foreach Menu


            return null;
        }
    }
}

