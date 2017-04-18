using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Modelos.Web.ViewModel
{
    public class AlterarOuSalvarUsuarioView : IViewModel<Profissional>
    {
        public string CodigoUsuario { get; set; }

        public IList<string> CodigoOuSigla { get; set; }

        public string Nome { get; set; }

        public string ActionDestino { get; set; }

        /// <summary>
        /// Pegar os dados do usuário para exibir na Tela de Alteração (ou cadastro)
        /// </summary>
        /// <param name="usuario">Usuário que será alterado</param>
        public void PreencherDadosView(Profissional usuario)
        {
            this.CodigoUsuario = usuario.Codigo;
            this.Nome = usuario.Nome;
            this.CodigoOuSigla = new List<string>();

            if (usuario.OrgaoPadrao != null)
                this.CodigoOuSigla.Add(usuario.OrgaoPadrao.Sigla);

            if (usuario.OrgaoPadrao != null)
            {

                foreach (Orgao o in usuario.OrgaosDoUsuario)
                {
                    if (usuario.OrgaoPadrao != null)
                    {
                        if (o.Sigla == usuario.OrgaoPadrao.Sigla ||
                            o.Codigo == usuario.OrgaoPadrao.Codigo)
                            continue;
                    }

                    this.CodigoOuSigla.Add(o.Sigla);
                }
            }

            if (this.CodigoOuSigla == null || this.CodigoOuSigla.Count == 0)
            {
                this.CodigoOuSigla = new List<string>();
                this.CodigoOuSigla.Add("");
            }
        }

        public Profissional CriarModelo()
        {
            Profissional retorno = new Profissional
            {
                Codigo = this.CodigoUsuario,
                Nome = this.Nome
            };

            if (this.CodigoOuSigla != null && this.CodigoOuSigla.Count > 0)
            {
                retorno.OrgaoPadrao = new Orgao { Sigla = this.CodigoOuSigla[0], Codigo = this.CodigoOuSigla[0] };
                retorno.OrgaosDoUsuario = new List<Orgao>();
                retorno.OrgaosDoUsuario.Add(retorno.OrgaoPadrao);

                for (int i = 1; i < this.CodigoOuSigla.Count; i++)
                {
                    retorno.OrgaosDoUsuario.Add(new Orgao
                    {
                        Sigla = this.CodigoOuSigla[i],
                        Codigo = this.CodigoOuSigla[i]
                    });
                }
            }

            return retorno;
        }
    }
}


