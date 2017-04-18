using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Facade_Modelos.Web
{
    public class CopiaDeAutorizacao
    {
        /// <summary>
        /// Autorizacões do Usuario de Origem (que se deseja ter as permissões a copiar)
        /// </summary>
        public Autorizacao AutorizacaoOrigem { get; set; }

        /// <summary>
        /// Autorizações do Usuário de Destino (que se deseja receber as permissões do origem)
        /// </summary>
        public Autorizacao AutorizacaoDestino { get; set; }


    }
}