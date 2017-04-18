using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Modelos.Web
{
    public class Autorizacao
    {
        public Profissional Usuario { get; set; }

        public Profissional UsuarioAutorizando { get; set; }

        public Aplicativo Aplicativo { get; set; }

        public Orgao OrgaoAutorizado { get; set; }


    }
}
