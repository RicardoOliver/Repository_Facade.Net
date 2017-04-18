using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Modelos.Web
{
    public class Menu
    {
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public int Posicao { get; set; }

        public IList<SubMenu> SubMenus { get; set; }

    }
}