using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Modelos.Web
{
    public class Liberacao
    {
        public DateTime DataDeLiberacao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Observacao { get; set; }
        public Profissional UsuarioLiberacao { get; set; }
        public Permissoes Permissoes { get; set; }

    }
}
