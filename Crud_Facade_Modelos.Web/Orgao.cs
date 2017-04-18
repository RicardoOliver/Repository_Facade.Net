using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Modelos.Web
{
    public class Orgao
    {
        /// <summary>
        /// Sigla do Órgão. Exemplo UTU2
        /// </summary>
        public string Sigla { get; set; }

        /// <summary>
        /// Nome do Órgão Responsável pelo processo. <br />
        /// Ex: SUBSECRETARIA DA SEXTA TURMA
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Código do Órgão responsável
        /// </summary>
        public string Codigo { get; set; }
    }
}
