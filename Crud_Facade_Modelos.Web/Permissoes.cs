using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Modelos.Web
{
    public class Permissoes
    {
        /// <summary>
        /// Indica se o usuário tem permissão para alterar
        /// </summary>
        public bool PermissaoAlterar { get; set; }

        /// <summary>
        /// Indica se o usuario tem permissão para excluir
        /// </summary>
        public bool PermissaoExcluir { get; set; }

        /// <summary>
        /// Indica se o usuario tem permissao para incluir
        /// </summary>
        public bool PermissaoIncluir { get; set; }


    }
}
