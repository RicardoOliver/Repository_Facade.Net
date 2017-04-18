using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Modelos.Web.ViewModel
{
    public class AlterarOuSalvarAplicativoView : IViewModel<Aplicativo>
    {
        public string NomeAplicacao { get; set; }

        public string Descricao { get; set; }

        public string DescricaoSintetica { get; set; }

        public string Versao { get; set; }

        public string URLDeExecucao { get; set; }

        public string Plataforma { get; set; }

        public string URLDeLogoff { get; set; }

        public string ActionDestinos { get; set; }

        public IList<Menu> Menus { get; set; }


        public void PreencherDadosView(Aplicativo dados)
        {
            this.NomeAplicacao = dados.Nome;
            this.Descricao = dados.Descricao;
            this.DescricaoSintetica = dados.DescricaoSintetica;
            this.Versao = dados.Versao;
            this.URLDeExecucao = dados.URLDeExecucao;
            this.URLDeLogoff = dados.URLDeLogoff;
            this.Plataforma = dados.Plataforma;
            this.Menus = new List<Menu>();
            if (dados.Nome != null)
            {
                this.Menus.Add(new Menu());
            }
            if (dados.Descricao != null)
            {
                this.Menus.Add(new Menu());
            }

        }

        public Aplicativo CriarModelo()
        {
            Aplicativo retorno = new Aplicativo();

            retorno.Nome = NomeAplicacao;
            retorno.Descricao = Descricao;
            retorno.DescricaoSintetica = DescricaoSintetica;
            retorno.Versao = Versao;
            retorno.URLDeExecucao = URLDeExecucao;
            retorno.URLDeLogoff = URLDeLogoff;
            retorno.Plataforma = Plataforma;

            if (this.Menus != null)
            {
                retorno.Menus = Menus;
                int desktop = Plataforma == "D" ? 1 : 0;
                for (int i = 0; i < retorno.Menus.Count; i++)
                {
                    Menu m = retorno.Menus[i];

                    m.Posicao = i + desktop;

                    if (m.SubMenus != null)
                    {
                        for (int j = 0; j < m.SubMenus.Count; j++)
                        {
                            SubMenu s = m.SubMenus[j];

                            s.Posicao = j + 1;

                        }
                    } // if

                } // for
            } // if


            return retorno;

        }
    }
}



