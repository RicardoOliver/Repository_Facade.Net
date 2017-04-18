using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Modelos.Web
{
    public class Aplicativo
    {

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string DescricaoSintetica { get; set; }

        public string Versao { get; set; }

        public string URLDeExecucao { get; set; }

        public string Plataforma { get; set; }

        public string URLDeLogoff { get; set; }



        public IList<Menu> Menus { get; set; }



    }
}

